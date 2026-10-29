using InputModelMapperLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InputModelMapperLab.Infrastructure.Data;

public sealed class EmployeeOperationsDbContext : DbContext
{
    public EmployeeOperationsDbContext(
        DbContextOptions<EmployeeOperationsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.DepartmentId);
            entity.Property(d => d.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasData(
                new Department { DepartmentId = 1, Name = "Engineering" },
                new Department { DepartmentId = 2, Name = "Human Resources" },
                new Department { DepartmentId = 3, Name = "Finance" });
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.EmployeeNumber)
                .HasMaxLength(30)
                .IsRequired();
            entity.HasIndex(e => e.EmployeeNumber).IsUnique();

            entity.Property(e => e.FirstName)
                .HasMaxLength(80).IsRequired();
            entity.Property(e => e.LastName)
                .HasMaxLength(80).IsRequired();
            entity.Property(e => e.Email)
                .HasMaxLength(200).IsRequired();
            entity.Property(e => e.HireDate).HasColumnType("date");
            entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
            entity.Property(e => e.EmploymentStatus)
                .HasMaxLength(30).IsRequired();
            entity.Property(e => e.CreatedAtUtc)
                .HasColumnType("datetime2");

            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
