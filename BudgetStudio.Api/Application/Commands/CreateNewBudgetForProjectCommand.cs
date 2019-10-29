using System;
using MediatR;
using Newtonsoft.Json;

namespace BudgetStudio.Api.Application.Commands
{
	public class CreateNewBudgetForProjectCommand : IRequest<Guid>
	{
		public Guid ProjectId { get; }

		[JsonConstructor]
		public CreateNewBudgetForProjectCommand(Guid projectId)
		{
			ProjectId = projectId;
		}
	}
}