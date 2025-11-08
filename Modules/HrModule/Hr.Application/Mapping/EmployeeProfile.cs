using AutoMapper;
using Hr.Application.DTOs;
using Hr.Domain.Entities;

namespace Hr.Application.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            
            CreateMap<EmployeeDto, Employee>();
        }
    }
}
