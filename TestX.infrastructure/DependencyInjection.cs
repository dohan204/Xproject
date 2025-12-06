using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TestX.application.InterfacesContext;
using TestX.domain.Entities.AccountRole;
using TestX.infrastructure.Identity;
namespace TestX.infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add infrastructure services here

            services.AddDbContext<IdentityContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddHttpContextAccessor();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();
            //services.AddScoped<IApplicationDbContext>(required =>
            //required.GetRequiredService<IdentityContext>());
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisCacheOptions:Configuration"];
                options.InstanceName = configuration["RedisCacheOptions:InstanceName"];
            });
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConfig = configuration["RedisCacheOptions:Configuration"];
                if (string.IsNullOrEmpty(redisConfig))
                {
                    throw new ArgumentNullException("Redis configuration is missing in appsetting.json");
                }
                return ConnectionMultiplexer.Connect(redisConfig);
            });

            //services.AddDbContext<TempIdentityContext>(options =>
            //   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddIdentity<ApplicationUser, ApplicationRole>()
            //    .AddEntityFrameworkStores<TempIdentityContext>()
            //    .AddDefaultTokenProviders();
        }
    }
}
