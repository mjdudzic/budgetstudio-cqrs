using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Api.Infrastructure.Persistence
{
	public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.ToTable("employees");

			builder.HasKey(i => i.Id);
			builder
				.Property(i => i.Id)
				.HasColumnName("id");

			builder
				.Property(i => i.FirstName)
				.HasColumnName("first_name");

			builder
				.Property(i => i.LastName)
				.HasColumnName("last_name");

			builder
				.Property(i => i.JobTitle)
				.HasColumnName("job_title");

			builder
				.Property(i => i.EmploymentType)
				.HasColumnName("employment_type");

			builder
				.Property(i => i.CreatedAt)
				.HasColumnName("created_at");

			builder
				.Property(i => i.UpdatedAt)
				.HasColumnName("updated_at");

			builder
				.Property(i => i.DeletedAt)
				.HasColumnName("deleted_at");

			builder
				.HasMany(i => i.TechSkills)
				.WithOne();
		}
	}
}