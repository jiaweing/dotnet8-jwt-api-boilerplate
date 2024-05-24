using Api.Application.Models;
using Api.Application.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Application.Services.Auth
{
	public class JwtService
    {
        private readonly RoleService _roleService;
        private readonly IConfiguration _configuration;

        public JwtService(
            RoleService roleService,
            IConfiguration configuration
        )
        {
            _roleService = roleService;
            _configuration = configuration;
        }

        public async Task<JwtAccessToken> CreateJwtAccessToken(User user)
        {
            var tokenExpirationMinutes = _configuration.GetValue("Api:Jwt:AccessTokenExpirationMinutes", 30);
            var expiration = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes);
            var token = CreateJwtSecurityToken(
                await CreateJwtAccessTokenClaimsAsync(user),
                CreateJwtSigningCredentials(),
                expiration
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            var JwtAccessToken = new JwtAccessToken()
            {
                AccessToken = tokenString,
                ExpiresAt = expiration
            };

            return JwtAccessToken;
        }

        private async Task<List<Claim>> CreateJwtAccessTokenClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)),
                new Claim("Nickname", user.Name),
                new Claim("UserId", user.UserId),
                new Claim("Email", user.EmailAddress.ToString()),
            };

            // add roles
            var roles = await _roleService.FindUserRolesAsync(user.UserId);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            return claims;
        }

        private JwtSecurityToken CreateJwtSecurityToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                _configuration.GetValue("Api:Jwt:Issuer", "example.com"),
                _configuration.GetValue("Api:Jwt:Audience", "example.com"),
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private SigningCredentials CreateJwtSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration.GetValue("Api:Jwt:Key", "supersecretkey"))
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
