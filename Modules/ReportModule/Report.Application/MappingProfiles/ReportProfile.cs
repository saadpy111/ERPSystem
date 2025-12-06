using AutoMapper;
using Report.Application.DTOs;
using Report.Domain.Entities;

namespace Report.Application.MappingProfiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Domain.Entities.Report, ReportDto>()
                .ForMember(dest => dest.ReportDataSource, opt => opt.MapFrom(src => src.ReportDataSource))
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.Fields))
                .ForMember(dest => dest.Parameters, opt => opt.MapFrom(src => src.Parameters))
                .ForMember(dest => dest.Filters, opt => opt.MapFrom(src => src.Filters))
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.Groups))
                .ForMember(dest => dest.Sortings, opt => opt.MapFrom(src => src.Sortings));

            CreateMap<ReportDataSource, ReportDataSourceDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<ReportField, ReportFieldDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<ReportParameter, ReportParameterDto>()
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => src.DataType.ToString()));

            CreateMap<ReportFilter, ReportFilterDto>()
                .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.Operator.ToString()));

            CreateMap<ReportGroup, ReportGroupDto>();

            CreateMap<ReportSorting, ReportSortingDto>()
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction.ToString()));
        }
    }
}