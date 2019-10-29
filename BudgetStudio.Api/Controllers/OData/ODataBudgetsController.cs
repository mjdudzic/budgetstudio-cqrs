using BudgetStudio.Infrastructure.Persistence;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BudgetStudio.Api.Controllers.OData
{
	[ODataRoutePrefix("Budgets")]
	public class ODataBudgetsController : ODataController
	{
		private readonly BudgetContext _budgetContext;

		public ODataBudgetsController(BudgetContext budgetContext)
		{
			_budgetContext = budgetContext;
		}

		[HttpGet]
		[ODataRoute]
		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(_budgetContext.BudgetViewItems);
		}
	}
}