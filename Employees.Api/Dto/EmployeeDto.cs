using System;
using System.Collections.Generic;

namespace Employees.Api.Dto
{
	public class EmployeeDto
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string JobTitle { get; set; }
		public string EmploymentType { get; set; }
		public List<string> TechSkills { get; set; }
	}
}