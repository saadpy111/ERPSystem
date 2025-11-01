using AutoMapper;
using Hr.Application.DTOs;
using Hr.Domain.Entities;

namespace Hr.Application.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
            
            CreateMap<EmployeeDto, Employee>();
        }
    }
}
