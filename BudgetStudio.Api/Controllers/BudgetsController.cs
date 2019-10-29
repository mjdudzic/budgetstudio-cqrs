using System;
using System.Threading.Tasks;
using BudgetStudio.Api.Application.Commands;
using BudgetStudio.Api.Application.Queries;
using BudgetStudio.Api.Models;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetStudio.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BudgetsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BudgetsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetBudget(Guid id)
		{
			var result = await _mediator.Send(new GetBudgetQuery(id));

			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> CreateBudgetForProject(CreateNewBudgetForProjectCommand command)
		{
			var budgetId = await _mediator.Send(command);
			
			return CreatedAtAction(nameof(GetBudget), new {id = budgetId }, new { budgetId });
		}

		[HttpPost("{budgetId}/employeecosts")]
		public async Task<IActionResult> AddEmployeeCost(Guid budgetId, AddEmployeeCostModel model)
		{
			await _mediator.Send(new AddEmployeeCostToBudgetCommand(
				budgetId,
				new EmployeeIdentity(model.EmployeeCode),
				new Participation(model.StartedAt, model.EndedAt)));

			return AcceptedAtAction(nameof(GetBudget), new { id = budgetId }, new { budgetId });
		}
	}
}
