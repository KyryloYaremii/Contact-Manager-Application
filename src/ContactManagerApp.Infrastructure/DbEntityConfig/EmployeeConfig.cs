using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContactManagerApp.Domain.Entities;

namespace ContactManagerApp.Infrastructure.Config;

public class EmployeeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.DateOfBirth).HasColumnType("date");
        builder.Property(x => x.Married);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.Salary).HasColumnType("decimal(18,2)");
    }
}