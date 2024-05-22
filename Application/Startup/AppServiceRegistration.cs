using Api.Application.Database;
using Api.Application.Services.Auth;
using System.Text.Json.Serialization;

namespace Api.Application.Startup
{
    public static class AppServiceRegistration
    {
        public static async Task<IServiceCollection> AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddJwt(config);

            services.AddSwagger();
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRateLimit(config);
            services.AddDatabase<DatabaseContext>(config);
            services.AddCustomServices();

            return services;
        }

        private static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserService>();
            return services;
        }
    }
}
