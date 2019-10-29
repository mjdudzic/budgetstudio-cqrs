using System;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using MediatR;

namespace BudgetStudio.Api.Application.Commands
{
	public class AddEmployeeCostToBudgetCommand : IRequest
	{
		public Guid BudgetId { get; }
		public EmployeeIdentity EmployeeIdentity { get; }
		public Participation Participation { get; }

		public AddEmployeeCostToBudgetCommand(
			Guid budgetId,
			EmployeeIdentity employeeIdentity,
			Participation participation)
		{
			BudgetId = budgetId;
			EmployeeIdentity = employeeIdentity;
			Participation = participation;
		}
	}
}