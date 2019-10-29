using System;
using System.Collections.Generic;

namespace Employees.Api.Infrastructure.Persistence
{
	public class Employee
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string JobTitle { get; set; }
		public string EmploymentType { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public DateTime DeletedAt { get; set; }

		public virtual ICollection<TechSkill> TechSkills { get; set; }
	}
}