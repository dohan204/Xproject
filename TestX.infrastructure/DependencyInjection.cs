using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IApplicationDbContext>(required =>
            required.GetRequiredService<IdentityContext>());
        }
    }
}
