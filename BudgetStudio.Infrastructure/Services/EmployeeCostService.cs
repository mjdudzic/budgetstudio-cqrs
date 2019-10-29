using System;
using System.Threading.Tasks;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;

namespace BudgetStudio.Infrastructure.Services
{
	public class EmployeeCostService : IEmployeeCostService
	{
		public async Task<Price> GetEmployeeCostAsync(string employeeCode)
		{
			await Task.Delay(0);

			return new Price(new Random().Next(1000, 10000), "PLN");
		}
	}
}