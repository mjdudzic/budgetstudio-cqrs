using System.Linq;
using AutoMapper;
using BudgetStudio.Domain.AggregatesModel.BudgetAggregate;
using BudgetStudio.ViewModels;

namespace BudgetStudio.Api.MapperProfiles
{
	public class BudgetMapperProfile : Profile
	{
		public BudgetMapperProfile()
		{
			CreateMap<Budget, BudgetViewModel>()
				.ForMember(dest => dest.BudgetId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.TotalCostAmount, opt => opt.MapFrom(src => src.TotalCost.Amount))
				.ForMember(dest => dest.TotalCostCurrency, opt => opt.MapFrom(src => src.TotalCost.Currency))
				.ForMember(dest => dest.EmployeeCosts, opt => opt.MapFrom(src => src.EmployeeCosts.Select(i => new EmployeeCostModel
				{
					EmployeeCode = i.EmployeeCode,
					CostAmount = i.Cost.Amount,
					CostCurrency = i.Cost.Currency,
					ParticipationStartedAt = i.Participation.StartedAt,
					ParticipationEndedAt = i.Participation.EndedAt
				})))
				.ForMember(dest => dest.ExtraCosts, opt => opt.MapFrom(src => src.ExtraCosts.Select(i => new ExtraCostModel
				{
					Description = i.Description,
					CostAmount = i.Cost.Amount,
					CostCurrency = i.Cost.Currency,
				})));
		}
	}
}