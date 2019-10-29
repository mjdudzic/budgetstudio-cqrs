using System.Collections.Generic;
using BudgetStudio.ViewModels;
using MediatR;

namespace BudgetStudio.Api.Application.Queries
{
	public class GetBudgetTopXByTotalCostQuery : IRequest<List<BudgetCostReportViewModel>>
	{
		public int CountThreshold { get; }
		public string Currency { get; }

		public GetBudgetTopXByTotalCostQuery(int countThreshold, string currency)
		{
			CountThreshold = countThreshold;
			Currency = currency;
		}
	}
}