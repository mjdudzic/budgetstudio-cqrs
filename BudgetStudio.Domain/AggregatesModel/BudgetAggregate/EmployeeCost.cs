using System.Collections.Generic;
using BudgetStudio.Domain.SeedWork;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public class EmployeeCost : ValueObject
	{
		public string EmployeeCode { get; set; }
		public Participation Participation { get; set; }
		public Price Cost { get; set; }
		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return EmployeeCode;
			yield return Participation;
		}
	}
}