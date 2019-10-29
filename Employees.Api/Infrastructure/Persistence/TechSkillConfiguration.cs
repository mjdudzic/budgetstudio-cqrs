using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Api.Infrastructure.Persistence
{
	public class TechSkillConfiguration : IEntityTypeConfiguration<TechSkill>
	{
		public void Configure(EntityTypeBuilder<TechSkill> builder)
		{
			builder.ToTable("tech_skills");

			builder.HasKey(i => i.Id);
			builder
				.Property(i => i.Id)
				.HasColumnName("id");

			builder
				.Property(i => i.Description)
				.HasColumnName("description");

			builder
				.Property("EmployeeId")
				.HasColumnName("employee_id");
		}
	}
}