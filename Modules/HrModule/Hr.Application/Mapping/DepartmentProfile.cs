using AutoMapper;
using Hr.Application.DTOs;
using Hr.Domain.Entities;

namespace Hr.Application.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
        }
    }
}
