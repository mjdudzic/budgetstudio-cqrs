using System.Collections.Generic;
using BudgetStudio.Domain.SeedWork;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public class Price : ValueObject
	{
		public decimal Amount { get; private set; }
		public string Currency { get; private set; }

		private Price() {}

		public Price(decimal amount, string currency)
		{
			Amount = amount;
			Currency = currency;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Amount;
			yield return Currency;
		}
	}
}