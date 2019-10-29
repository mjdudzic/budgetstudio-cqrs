using System;
using System.Threading.Tasks;
using AutoMapper;
using Employees.Api.Dto;
using Employees.Api.Infrastructure.Persistence;
using Employees.Api.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Employees.Api.Application
{
	public class EmployeeService : IEmployeeService
	{
		private readonly EmployeesContext _employeesContext;
		private readonly IMapper _mapper;
		private readonly ILogger<EmployeeService> _logger;

		public EmployeeService(
			EmployeesContext employeesContext,
			IMapper mapper,
			ILogger<EmployeeService> logger)
		{
			_employeesContext = employeesContext;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<EmployeeDto> CreateNewEmployeeAsync(CreateEmployeeModel createEmployeeModel)
		{
			_logger.LogInformation(
				"Adding new employee {createEmployeeModel}", 
				JsonConvert.SerializeObject(createEmployeeModel));

			// Run some validation rules, simple RPCs ...
			await Task.CompletedTask;

			var entity = _mapper.Map<Employee>(createEmployeeModel);

			_employeesContext.Employees.Add(entity);

			await _employeesContext.SaveChangesAsync();

			return _mapper.Map<EmployeeDto>(entity);
		}

		public async Task UpdateEmployeeAsync(Guid id, UpdateEmployeeModel updateEmployeeModel)
		{
			_logger.LogInformation(
				"Updating employee {id} - {updateEmployeeModel}",
				id,
				JsonConvert.SerializeObject(updateEmployeeModel));

			// Run some validation rules, simple RPCs ...
			await Task.CompletedTask;

			var entity = await _employeesContext.Employees.FindAsync(id);
			if (entity == null)
			{
				return;
			}

			entity.TechSkills.Clear();
			updateEmployeeModel
				.TechSkills
				.ForEach(i => entity.TechSkills.Add(new TechSkill { Description = i }));

			_mapper.Map(updateEmployeeModel, entity);

			await _employeesContext.SaveChangesAsync();
		}

		public async Task DeleteEmployeeAsync(Guid id)
		{
			_logger.LogInformation("Deleting employee {id}", id);

			// Run some validation rules, simple RPCs ...
			await Task.CompletedTask;

			var entity = await _employeesContext.Employees.FindAsync(id);
			if (entity == null)
			{
				return;
			}

			entity.DeletedAt = DateTime.UtcNow;

			await _employeesContext.SaveChangesAsync();
		}
	}
}