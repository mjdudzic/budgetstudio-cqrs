using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BudgetStudio.ViewModels;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BudgetStudio.Api.Application.Queries
{
	public class GetEmployeesParticipationCountQueryHandler 
		: IRequestHandler<GetEmployeesParticipationCountQuery, IEnumerable<EmployeeParticipationReportViewModel>>
	{
		private readonly IConfiguration _configuration;

		public GetEmployeesParticipationCountQueryHandler(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IEnumerable<EmployeeParticipationReportViewModel>> Handle(
			GetEmployeesParticipationCountQuery request,
			CancellationToken cancellationToken)
		{
			var connectionString = _configuration["ConnectionStrings:PostgreSql"];

			using (var connection = new NpgsqlConnection(connectionString))
			{
				connection.Open();
				var query =
					$@"SELECT
						 emp.employee_code AS {nameof(EmployeeParticipationReportViewModel.EmployeeCode)}
						,COUNT(1) AS {nameof(EmployeeParticipationReportViewModel.ProjectParticipationCount)}
						,AVG(emp.cost_amount) AS {nameof(EmployeeParticipationReportViewModel.AvgCost)}
						FROM employee_costs AS emp
						WHERE emp.participation_started_at >= @ThresholdStartDate
						AND emp.participation_started_at < @ThresholdEndDate
						GROUP BY emp.employee_code ";

				return await connection.QueryAsync<EmployeeParticipationReportViewModel>(
					query,
					new {request.ThresholdStartDate, request.ThresholdEndDate});
			}
		}
	}
}