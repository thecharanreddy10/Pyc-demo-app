using Microsoft.EntityFrameworkCore;
using PayrollApp.Models;

namespace PayrollApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<PayrollRecord> PayrollRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Employee entity
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EmployeeId).IsUnique();
            entity.Property(e => e.AnnualSalary).HasColumnType("decimal(18,2)");
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");

            // One-to-many relationship: Employee -> PayrollRecords
            entity.HasMany(e => e.PayrollRecords)
                  .WithOne(p => p.Employee)
                  .HasForeignKey(p => p.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure PayrollRecord entity
        modelBuilder.Entity<PayrollRecord>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.HoursWorked).HasColumnType("decimal(18,2)");
        });
    }
}
