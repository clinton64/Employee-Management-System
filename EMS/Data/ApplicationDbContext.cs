using EMS.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                 .HasMany(e => e.Employees)
                 .WithOne(e => e.Project)
                 .HasForeignKey(e => e.ProjectId)
                 .HasPrincipalKey(e => e.ProjectId);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeEmail)
                .IsUnique();
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Phone)
                .IsUnique();
        }
    }
}
