using System;
using System.Collections.Generic;
using BudgetStudio.Domain.SeedWork;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public class Participation : ValueObject
	{
		public DateTime StartedAt { get; private set; }
		public DateTime EndedAt { get; private set; }

		private Participation() { }

		public Participation(DateTime startedAt, DateTime endedAt)
		{
			StartedAt = startedAt;
			EndedAt = endedAt;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return StartedAt;
			yield return EndedAt;
		}
	}
}