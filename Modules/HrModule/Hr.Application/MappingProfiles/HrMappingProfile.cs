using AutoMapper;
using Hr.Application.DTOs;
using Hr.Domain.Entities;

namespace Hr.Application.MappingProfiles
{
    public class HrMappingProfile : Profile
    {
        public HrMappingProfile()
        {
            // Department mappings
            CreateMap<Department, DepartmentDto>()
                .ReverseMap();

            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ReverseMap();

            // Job mappings
            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ReverseMap();

            // LeaveRequest mappings
            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ReverseMap();

            // Loan mappings
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ReverseMap();

            // AttendanceRecord mappings
            CreateMap<AttendanceRecord, AttendanceRecordDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ReverseMap();

            // PayrollRecord mappings
            CreateMap<PayrollRecord, PayrollRecordDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ReverseMap();

            // Applicant mappings
            CreateMap<Applicant, ApplicantDto>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.AppliedJob != null ? src.AppliedJob.Title : null))
                .ForMember(dest => dest.CurrentStageName, opt => opt.MapFrom(src => src.CurrentStage != null ? src.CurrentStage.Name : null))
                .ReverseMap();

            // RecruitmentStage mappings
            CreateMap<RecruitmentStage, RecruitmentStageDto>()
                .ReverseMap();

            // LoanInstallment mappings
            CreateMap<LoanInstallment, LoanInstallmentDto>()
                .ReverseMap();

            // PayrollComponent mappings
            CreateMap<PayrollComponent, PayrollComponentDto>()
                .ReverseMap();
        }
    }
}
