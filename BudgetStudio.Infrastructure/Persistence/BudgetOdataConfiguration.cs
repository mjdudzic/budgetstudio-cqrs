using BudgetStudio.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetStudio.Infrastructure.Persistence
{
	public class BudgetOdataConfiguration : IEntityTypeConfiguration<BudgetOdataViewModel>
	{
		public void Configure(EntityTypeBuilder<BudgetOdataViewModel> builder)
		{
			builder.ToTable("budgets_view");
			builder.HasKey(i => i.BudgetId);
			builder.Property(i => i.BudgetId).HasColumnName("budget_id");
			builder.Property(i => i.ProjectId).HasColumnName("project_id");
			builder.Property(i => i.ConfirmedAt).HasColumnName("confirmed_at");
			builder.Property(i => i.CreatedAt).HasColumnName("created_at");
			builder.Property(i => i.EmployeesCount).HasColumnName("employees_count");
			builder.Property(i => i.EmployeesTotalCost).HasColumnName("employees_total_cost");
			builder.Property(i => i.ExtraCostsCount).HasColumnName("extra_costs_count");
			builder.Property(i => i.ExtraCostsTotalCost).HasColumnName("extra_costs_total_cost");
			builder.Property(i => i.RejectedAt).HasColumnName("rejected_at");
			builder.Property(i => i.RejectionReason).HasColumnName("rejection_reason");
			builder.Property(i => i.TotalCostAmount).HasColumnName("total_cost_amount");
			builder.Property(i => i.TotalCostCurrency).HasColumnName("total_cost_currency");
		}
	}
}