using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using TestX.application.InterfacesContext;
using TestX.domain.Entities.AccountRole;
using TestX.domain.Entities.General;


namespace TestX.infrastructure.Identity
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
        public DbSet<Province> Provinces { get; set; }
        //public DbSet<AccountPermission> AccountPermissions { get; set; }
        public DbSet<WardsCommune> WardsCommunes { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AccountPermission> AccountPermissions { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<SchoolLevel> SchoolLevel { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<ExamDetails> ExamDetails { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentExamDetails> StudentExamDetails { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<ChoiceExam> Choices { get; set; }
        public DbSet<Topic> Topics { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // User - Province (1-1)
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Province)
                .WithMany()
                .HasForeignKey(p => p.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);
            // Province - WardsCommunes (1-n)
            builder.Entity<ApplicationUser>()
                .HasOne(e => e.StudentExam)
                .WithOne(se => se.ApplicationUser)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Province>()
                .HasMany(w => w.WardsCommune)
                .WithOne(p => p.Province)
                .HasForeignKey(p => p.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.WardsCommune)
                .WithMany()
                .HasForeignKey(u => u.WardsCommuneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationRole>()
                .HasMany(e => e.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .HasConstraintName("FK_RolePermissions_Role")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RolePermission>()
                .HasKey(rp => rp.Id);
            builder.Entity<Function>()
                .HasOne(m => m.Module)
                .WithMany(f => f.Functions)
                .HasForeignKey(m => m.ModuleId)
                .HasConstraintName("FK_Functions_Module")
                .IsRequired();

            builder.Entity<Module>()
                .HasKey(m => m.Id);
            builder.Entity<Function>()
                .HasMany(ac => ac.AccountPermissions)
                .WithOne()
                .HasForeignKey(ap => ap.FunctionId)
                .HasConstraintName("FK_AccountPermissions_Function")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Function)
                .WithMany(e => e.RolePermissions)
                .HasForeignKey(rp => rp.FunctionId)
                .HasConstraintName("FK_RolePermissions_Function")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SchoolLevel>()
                .HasKey(sl => sl.Id);
            builder.Entity<SchoolLevel>()
                .HasMany(ac => ac.Schools)
                .WithOne(s => s.SchoolLevel)
                .HasForeignKey(s => s.SchoolLevelId)
                .HasConstraintName("FK_Schools_SchoolLevel")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentExam>()
                .HasMany(se => se.StudentExamDetails)
                .WithOne(sed => sed.StudentExam)
                .HasForeignKey(se => se.StudentExamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Exam>()
                .HasMany(e => e.StudentExams)
                .WithOne(se => se.Exam)
                .HasForeignKey(se => se.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Exam>()
                .HasMany(e => e.ExamDetails)
                .WithOne(e => e.Exam)
                .HasForeignKey(e => e.ExamId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ExamDetails>()
                .HasOne(e => e.Question)
                .WithMany(e => e.ExamDetails)
                .HasConstraintName("FK_ExamDetails_Question")
                .HasForeignKey(e => e.QuestionId);

            builder.Entity<Subject>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Subject)
                .HasForeignKey(q => q.SubjectId)
                .HasConstraintName("FK_Questions_Subject");


            builder.Entity<QuestionType>()
                .HasMany(e => e.Questions)
                .WithOne(q => q.QuestionType)
                .HasForeignKey(e => e.QuestionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Level>()
                .HasMany(e => e.Questions)
                .WithOne(l => l.Level)
                .HasForeignKey(e => e.LevelId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<History>()
                .HasMany(e => e.StudentExamDetails)
                .WithOne(htr => htr.History)
                .HasForeignKey(e => e.HistoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChoiceExam>()
                .HasKey(key => new { key.AccountId, key.HistoryId, key.ExamId });


            builder.Entity<ChoiceExam>()
                .HasOne(e => e.History)
                .WithMany(c => c.ChoiceExams)
                .HasForeignKey(e => e.HistoryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<ChoiceExam>()
                .HasOne(e => e.Exam)
                .WithMany()
                .HasForeignKey(e => e.ExamId)
                .OnDelete(DeleteBehavior.Restrict);
            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                // Kiểm tra xem hành vi xóa có phải là Cascade theo mặc định (nếu bạn chưa đặt Restrict ở mọi nơi)
                // và thay đổi nó nếu cần để tránh lỗi cycles.
                // Dòng mã dưới đây tìm tất cả các khóa ngoại mà không được cấu hình rõ ràng
                // và đặt chúng thành Restrict nếu chúng là bắt buộc.
                if (foreignKey.DeleteBehavior == DeleteBehavior.Cascade)
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }

                // Hoặc cụ thể hơn, nếu lỗi chỉ liên quan đến các bảng AspNet* mặc định:
                // if (foreignKey.PrincipalEntityType.Name.Contains("Identity") || foreignKey.PrincipalEntityType.Name.Contains("Application"))
                // {
                //     foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                // }
            }

            // Một cách tiếp cận khác là sử dụng DeleteBehavior.ClientSetNull nếu các cột khóa ngoại là nullable.
            // Nếu các khóa ngoại không phải nullable (như trong AspNetUsers), hãy dùng Restrict hoặc NoAction.
        }
    }
}