using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestX.domain.Entities.AccountRole;
using TestX.domain.Entities.General;

namespace TestX.application.InterfacesContext
{
    public interface IApplicationDbContext
    {
        DbSet<Province> Provinces { get; set; }
        DbSet<WardsCommune> WardsCommunes { get; set; }
        DbSet<Module> Modules { get; set; }
        DbSet<Function> Functions { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<AccountPermission> AccountPermissions { get; set; }
        DbSet<School> School { get; set; }
        DbSet<SchoolLevel> SchoolLevel { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
