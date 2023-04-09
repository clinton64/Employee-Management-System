using EMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        //public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Employees)
                .WithOne(e => e.Project!)
                .HasForeignKey(e => e.ProjectId)
                .HasPrincipalKey(p => p.ProjectId);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Phone)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Image)
                .WithOne(i => i.Employee!)
                .HasForeignKey<Image>(i => i.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
