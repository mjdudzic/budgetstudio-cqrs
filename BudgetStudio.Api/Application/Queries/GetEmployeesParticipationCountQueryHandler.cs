using System.Collections.Generic;
using System.Data.SqlClient;
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
			var useDb = _configuration["ConnectionStrings:Use"];

			if (useDb == "PostgreSql")
			{
				return await GetForPostgreSql(request);
			}

			return await GetForSqlServer(request);
		}

		private async Task<IEnumerable<EmployeeParticipationReportViewModel>> GetForPostgreSql(
			GetEmployeesParticipationCountQuery request)
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
					new { request.ThresholdStartDate, request.ThresholdEndDate });
			}
		}

		private async Task<IEnumerable<EmployeeParticipationReportViewModel>> GetForSqlServer(
			GetEmployeesParticipationCountQuery request)
		{
			var connectionString = _configuration["ConnectionStrings:SqlServer"];

			using (var connection = new SqlConnection(connectionString))
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
					new { request.ThresholdStartDate, request.ThresholdEndDate });
			}
		}
	}
}