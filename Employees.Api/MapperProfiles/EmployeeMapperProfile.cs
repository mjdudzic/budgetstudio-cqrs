using System;
using System.Linq;
using AutoMapper;
using Employees.Api.Dto;
using Employees.Api.Infrastructure.Persistence;
using Employees.Api.Models;

namespace Employees.Api.MapperProfiles
{
	public class EmployeeMapperProfile : Profile
	{
		public EmployeeMapperProfile()
		{
			CreateMap<Employee, EmployeeDto>()
				.ForMember(
					dest => dest.TechSkills, 
					opt => opt.MapFrom(src => src.TechSkills.Select(i => i.Description)));

			CreateMap<CreateEmployeeModel, Employee>()
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
				.ForMember(
					dest => dest.TechSkills, 
					opt => opt.MapFrom(src => src.TechSkills.Select(i => new TechSkill { Description = i})));

			CreateMap<UpdateEmployeeModel, Employee>()
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.TechSkills, opt => opt.Ignore());
		}
	}
}