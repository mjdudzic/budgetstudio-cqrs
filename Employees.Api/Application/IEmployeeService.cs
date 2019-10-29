using System;
using System.Threading.Tasks;
using Employees.Api.Dto;
using Employees.Api.Models;

namespace Employees.Api.Application
{
	public interface IEmployeeService
	{
		Task<EmployeeDto> CreateNewEmployeeAsync(CreateEmployeeModel createEmployeeModel);
		Task UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel);
		Task DeleteEmployeeAsync(Guid id);
	}
}