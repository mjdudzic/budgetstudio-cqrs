using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Employees.Api.Models
{
	public class CreateEmployeeModel
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string JobTitle { get; set; }
		[Required]
		public string EmploymentType { get; set; }
		public List<string> TechSkills { get; set; }
	}
}