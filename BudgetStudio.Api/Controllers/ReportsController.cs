using System;
using System.Threading.Tasks;
using BudgetStudio.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetStudio.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ReportsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("budgets")]
		public async Task<IActionResult> GetBudget(int take)
		{
			var result = await _mediator.Send(new GetBudgetTopXByTotalCostQuery(take, "PLN"));

			return Ok(result);
		}

		[HttpGet("employees")]
		public async Task<IActionResult> GetEmployees(DateTime startDate, DateTime endDate)
		{
			var result = await _mediator.Send(new GetEmployeesParticipationCountQuery(startDate, endDate));

			return Ok(result);
		}

		[HttpGet("extracosts")]
		public async Task<IActionResult> GetExtraCosts()
		{
			// TODO: complete query
			await Task.CompletedTask;

			return Ok();
		}
	}
}
