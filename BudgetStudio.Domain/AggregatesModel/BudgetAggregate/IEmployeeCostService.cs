using System.Threading.Tasks;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public interface IEmployeeCostService
	{
		Task<Price> GetEmployeeCostAsync(string employeeCode);
	}
}