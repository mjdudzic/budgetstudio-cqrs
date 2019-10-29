using Microsoft.EntityFrameworkCore;

namespace Employees.Api.Infrastructure.Persistence
{
	public class EmployeesContext : DbContext
	{
		public EmployeesContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<TechSkill> TechSkills { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
			modelBuilder.ApplyConfiguration(new TechSkillConfiguration());
		}
	}
}