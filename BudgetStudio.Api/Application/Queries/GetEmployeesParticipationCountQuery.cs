using System;
using System.Collections.Generic;
using BudgetStudio.ViewModels;
using MediatR;

namespace BudgetStudio.Api.Application.Queries
{
	public class GetEmployeesParticipationCountQuery : IRequest<IEnumerable<EmployeeParticipationReportViewModel>>
	{
		public DateTime ThresholdStartDate { get; }
		public DateTime ThresholdEndDate { get; }

		public GetEmployeesParticipationCountQuery(
			DateTime thresholdStartDate,
			DateTime thresholdEndDate)
		{
			ThresholdStartDate = thresholdStartDate;
			ThresholdEndDate = thresholdEndDate;
		}
	}
}