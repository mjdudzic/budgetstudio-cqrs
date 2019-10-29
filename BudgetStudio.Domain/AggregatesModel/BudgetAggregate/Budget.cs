using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetStudio.Domain.Events;
using BudgetStudio.Domain.SeedWork;

namespace BudgetStudio.Domain.AggregatesModel.BudgetAggregate
{
	public class Budget : Entity<Guid>, IAggregateRoot
	{
		private Guid _projectId;
		private DateTime _createdAt;
		private DateTime _confirmedAt;
		private string _rejectionReason;
		private DateTime _rejectedAt;
		private Price _totalCost;
		private readonly List<EmployeeCost> _employeeCosts;
		private readonly List<ExtraCost> _extraCosts;

		public Guid ProjectId => _projectId;
		public Price TotalCost => _totalCost;
		public DateTime CreatedAt => _createdAt;
		public DateTime? ConfirmedAt => _confirmedAt;
		public DateTime? RejectedAt => _rejectedAt;

		public IReadOnlyCollection<EmployeeCost> EmployeeCosts => _employeeCosts;
		public IReadOnlyCollection<ExtraCost> ExtraCosts => _extraCosts;


		private Budget()
		{
			_employeeCosts = new List<EmployeeCost>();
			_extraCosts = new List<ExtraCost>();
		}

		public Budget(
			Guid budgetId,
			Guid projectId) : this()
		{
			Id = budgetId;
			_projectId = projectId;

			_createdAt = DateTime.UtcNow;

			_totalCost = new Price(0, "PLN");
		}

		public async Task AddEmployeesCost(
			EmployeeIdentity employeeIdentity,
			Participation participation,
			IEmployeeCostService employeeCostService)
		{
			var employeeCost = new EmployeeCost
			{
				EmployeeCode = employeeIdentity.EmployeeCode,
				Participation = participation,
				Cost = await employeeCostService.GetEmployeeCostAsync(employeeIdentity.EmployeeCode)
			};

			_employeeCosts.Add(employeeCost);

			AddDomainEvent(new BudgetEmployeeCostDefinedEvent(Id, employeeCost));
		}

		public void AddExtraCost(ExtraCost extraCost)
		{
			_extraCosts.Add(extraCost);

			AddDomainEvent(new BudgetExtraCostDefinedEvent(Id, extraCost));
		}

		public async Task CalculateTotalCost(ICommissionCalculationService commissionCalculationService)
		{
			var employeeCostTotal = _employeeCosts.Sum(i => i.Cost.Amount);
			var extraCostTotal = _extraCosts.Sum(i => i.Cost.Amount);

			var budgetPrice = new Price(employeeCostTotal + extraCostTotal, "PLN");

			var commission = await commissionCalculationService.CalculateCommissionCostAsync(_projectId, budgetPrice);

			_totalCost = new Price(budgetPrice.Amount + commission.Amount, "PLN");

			AddDomainEvent(new BudgetTotalCostCalculatedEvent(Id, _totalCost));
		}

		public void Confirm()
		{
			_confirmedAt = DateTime.UtcNow;

			AddDomainEvent(new BudgetConfirmedEvent(Id, _confirmedAt));
		}

		public void Reject(string rejectionReason)
		{
			_rejectionReason = rejectionReason;
			_rejectedAt = DateTime.UtcNow;

			AddDomainEvent(new BudgetRejectedEvent(Id, _rejectedAt, rejectionReason));
		}
	}
}