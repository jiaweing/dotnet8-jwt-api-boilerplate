using Api.Application.Database;
using Api.Application.Models;
using Api.Application.Models.Auth;
using Api.Application.Models.Base;
using Api.Application.Models.OneOf;
using Api.Application.Utils;
using Api.Application.ViewModels;
using NanoidDotNet;
using OneOf;
using Serilog;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Application.Services.Auth
{
    public class AuthService
    {
        private readonly JwtService _jwtService;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly EmailUtils _emailUtils;

        private readonly DatabaseContext _db;

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly Serilog.ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public AuthService(
            JwtService jwtService,
            UserService userService,
            RoleService roleService,
            DatabaseContext db,
            IHttpContextAccessor contextAccessor,
            IConfiguration config,
            IWebHostEnvironment env,
            ILogger<AuthService> logger
        )
        {
            _jwtService = jwtService;
            _userService = userService;
            _roleService = roleService;

            _db = db;
            _contextAccessor = contextAccessor;
            _configuration = config;
            _emailUtils = new EmailUtils(env);
            _env = env;

            _logger = Log.ForContext<AuthService>();
        }

        public async Task<OneOf<Created<BaseResponse<JwtAccessToken>>, Error>> AuthenticateUserAsync(LoginDto dto)
        {
            var user = await _userService.FindUserAsync(dto.EmailAddress, _userService.GetSha256Hash(dto.Password));
            if (user is null)
            {
                _logger.Warning("{method}: Either user does not exist or password is incorrect.", nameof(AuthenticateUserAsync));
                return new Error("The credentials were incorrect. Try again.");
            }

            var token = await _jwtService.CreateJwtAccessToken(user);

            _logger.Information("{method}: Successfully authenticated user.", nameof(AuthenticateUserAsync));
            return new Created<BaseResponse<JwtAccessToken>>(new BaseResponse<JwtAccessToken>(token));
        }

        public async Task<OneOf<Created<BaseResponse<JwtAccessToken>>, BadRequest>> RegisterUserAsync(RegisterDto dto)
        {
            var user = await _userService.FindUserByEmailAsync(dto.EmailAddress);
            if (user != null)
            {
                _logger.Warning("{method}: User already exists.", nameof(RegisterUserAsync));
                return new BadRequest("A user with this email address already exists.");
            }

            if (dto.EmailAddress.Contains("+"))
            {
                _logger.Warning("{method}: Email contains a plus sign.", nameof(RegisterUserAsync));
                return new BadRequest("You cannot register with an alias email address.");
            }

            // check if email is in temp email list
            if (_emailUtils.IsEmailInTempList(dto.EmailAddress))
            {
                _logger.Warning("{method}: Email is in temp email list.", nameof(RegisterUserAsync));
                return new BadRequest("You cannot register with a temporary email address.");
            }

            string emailUsername = dto.EmailAddress.Split('@')[0];
            string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(emailUsername.Replace('.', ' '));

            var newUser = new User()
            {
                UserId = Nanoid.Generate(),
                EmailAddress = dto.EmailAddress,
                Name = name,
                Password = _userService.GetSha256Hash(dto.Password),
            };

            var addedUser = await _userService.CreateUserAsync(newUser, 2); // 2 == user role
            var token = await _jwtService.CreateJwtAccessToken(addedUser);

            _logger.Information("{method}: Successfully registered user.", nameof(RegisterUserAsync));
            return new Created<BaseResponse<JwtAccessToken>>(new BaseResponse<JwtAccessToken>(token));
        }

        public async Task<User?> GetAuthenticatedUser(ClaimsPrincipal User)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = GetAuthenticatedUserId(User);
                if (userId != null)
                {
                    var id = userId ?? default;
                    return await _userService.FindUserAsync(id);
                }
                return null;
            }
            return null;
        }

        public string? GetAuthenticatedUserId(ClaimsPrincipal User)
        {
            var userId = User.FindFirst("UserId")?.Value;
            return userId;
        }

        public string? GetAuthenticatedUserEmail(ClaimsPrincipal User)
        {
            var userId = User.FindFirst("Email")?.Value;
            return userId;
        }

        public string? GetAuthenticatedUserDisplayName(ClaimsPrincipal User)
        {
            var displayName = User.FindFirst("Nickname")?.Value;
            return displayName;
        }

        public bool IsAdmin(ClaimsPrincipal User)
        {
            return User.IsInRole(CustomRoles.Admin);
        }

        public string GetClaimsFromTokenString(string token, string claims)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var result = jwtToken.Claims.First(claim => claim.Type == claims).Value;
                return result;
            }

            catch (Exception e)
            {
                return null;
            }
        }
    }
}
