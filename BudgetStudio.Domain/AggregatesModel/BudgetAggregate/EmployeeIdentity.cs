using System.Collections.Generic;
using BudgetStudio.Domain.SeedWork;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public class EmployeeIdentity : ValueObject
	{
		public string EmployeeCode { get; private set; }

		private EmployeeIdentity() {}

		public EmployeeIdentity(string employeeCode)
		{
			EmployeeCode = employeeCode;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return EmployeeCode;
		}
	}
}