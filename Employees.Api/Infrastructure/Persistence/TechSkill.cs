using System;

namespace Employees.Api.Infrastructure.Persistence
{
	public class TechSkill
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public Guid EmployeeId { get; set; }
	}
}