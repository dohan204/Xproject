using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestX.domain.Entities.AccountRole;

public class TempIdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public TempIdentityContext(DbContextOptions<TempIdentityContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // CHỈ 1 DÒNG DUY NHẤT ĐỂ QUA MẶT SQL SERVER
        foreach (var fk in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}