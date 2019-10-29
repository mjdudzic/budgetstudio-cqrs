using System;
using BudgetStudio.ViewModels;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace BudgetStudio.Api.Application.Queries
{
	public class BudgetsODataModelBuilder
	{
		public IEdmModel GetEdmModel(IServiceProvider serviceProvider)
		{
			var builder = new ODataConventionModelBuilder(serviceProvider);
			builder.EnableLowerCamelCase();

			builder.EntitySet<BudgetOdataViewModel>("Budgets");

			return builder.GetEdmModel();
		}
	}
}