using Microsoft.EntityFrameworkCore;

namespace Api.Application.Startup
{
    public static class DatabaseServiceRegistration
    {
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, IConfiguration config) where T : DbContext
        {
            string host = config.GetValue<string>("Api:Database:Mysql:Host");
            string port = config.GetValue<string>("Api:Database:Mysql:Port");
            string database = config.GetValue<string>("Api:Database:Mysql:Database");
            string username = config.GetValue<string>("Api:Database:Mysql:Username");
            string password = config.GetValue<string>("Api:Database:Mysql:Password");
            string connectionString = $"server={host};port={port};database={database};user={username};password={password};";
            services.AddDbContext<T>(delegate (DbContextOptionsBuilder options)
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            return services;
        }
    }
}
