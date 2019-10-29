using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Employees.Api.Application;
using Employees.Api.Dto;
using Employees.Api.Infrastructure.Persistence;
using Employees.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Employees.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly EmployeesContext _employeesContext;
		private readonly IEmployeeService _employeeService;
		private readonly IMapper _mapper;
		private readonly ILogger<EmployeesController> _logger;

		public EmployeesController(
			EmployeesContext employeesContext,
			IEmployeeService employeeService,
			IMapper mapper,
			ILogger<EmployeesController> logger)
		{
			_employeesContext = employeesContext;
			_employeeService = employeeService;
			_mapper = mapper;
			_logger = logger;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Employee>> GetAll()
		{
			return _employeesContext.Employees;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<EmployeeDto>> GetEmployeeById(Guid id)
		{
			var entity = await _employeesContext.Employees.FindAsync(id);
			if (entity == null)
			{
				return NoContent();
			}

			entity.UpdatedAt = DateTime.UtcNow;
			DisplayStates(_employeesContext.ChangeTracker.Entries());
			return _mapper.Map<EmployeeDto>(entity);
		}

		[HttpPost]
		public async Task<IActionResult> AddNewEmployee(CreateEmployeeModel model)
		{
			var result = await _employeeService.CreateNewEmployeeAsync(model);

			return Ok(result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEmployeeData(Guid id, UpdateEmployeeModel model)
		{
			await _employeeService.UpdateEmployeeAsync(id, model);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEmployee(Guid id)
		{
			await _employeeService.DeleteEmployeeAsync(id);

			return NoContent();
		}

		private void DisplayStates(IEnumerable<EntityEntry> entries)
		{
			foreach (var entry in entries)
			{
				_logger.LogInformation($"Entity: {entry.Entity.GetType().Name}, State: { entry.State.ToString()}");
			}
		}
	}
}
