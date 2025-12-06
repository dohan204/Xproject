
using Microsoft.AspNetCore.Identity;
using TestX.domain.Entities.General;


namespace TestX.domain.Entities.AccountRole
{
    public class ApplicationUser : IdentityUser<string>
    {

        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Active { get; set; }
        public string? Gender { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ProvinceId { get; set; }
        public int? StudentExamId { get; set; }
        public StudentExam StudentExam { get; set; }
        public Province Province { get; set; }

        public int WardsCommuneId { get; set; }
        public WardsCommune WardsCommune { get; set; }
    }
}
