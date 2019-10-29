using System;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetStudio.Infrastructure.Persistence
{
	public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
	{
		public void Configure(EntityTypeBuilder<Budget> builder)
		{
			builder.ToTable("budgets");

			builder.Ignore(b => b.DomainEvents);

			builder.HasKey(i => i.Id);
			builder
				.Property(i => i.Id)
				.HasColumnName("id");

			builder.Property(i => i.ProjectId)
				.HasColumnName("project_id")
				.IsRequired();

			builder.Property(i => i.CreatedAt)
				.HasColumnName("created_at")
				.IsRequired();

			builder.Property(i => i.ConfirmedAt).HasColumnName("confirmed_at");
			builder.Property(i => i.RejectedAt).HasColumnName("rejected_at");

			builder.OwnsOne(i => i.TotalCost)
				.Property(i => i.Amount).HasColumnName("total_cost_amount");

			builder.OwnsOne(i => i.TotalCost)
				.Property(i => i.Currency).HasColumnName("total_cost_currency");

			var navigationEmployeeCosts = builder.Metadata.FindNavigation(nameof(Budget.EmployeeCosts));
			navigationEmployeeCosts.SetPropertyAccessMode(PropertyAccessMode.Field);

			var navigationExtraCosts = builder.Metadata.FindNavigation(nameof(Budget.ExtraCosts));

			navigationExtraCosts.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}