using System.Threading.RateLimiting;

namespace Api.Application.Startup
{
    public static class RateLimit
    {
        public static IServiceCollection AddRateLimit(this IServiceCollection services, IConfiguration config)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Request.Headers.Host.ToString(), partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = config.GetValue("Api:RateLimit:PermitLimit", 60),
                            AutoReplenishment = true,
                            Window = TimeSpan.FromSeconds(config.GetValue("Api:RateLimit:Window", 60))
                        });
                });

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken: token);
                };
            });

            return services;
        }
    }
}
