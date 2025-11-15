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
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null))
                .ReverseMap();

            // Department Detail mappings
            CreateMap<Department, DepartmentDetailDto>()
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null))
                .ForMember(dest => dest.Jobs, opt => opt.MapFrom(src => src.Jobs))
                .ReverseMap();

            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
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
                .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Contracts != null && src.Employee.Contracts.Any(c => c.IsActive) ? 
                    src.Employee.Contracts.First(c => c.IsActive).Job.Title : null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Contracts != null && src.Employee.Contracts.Any(c => c.IsActive) && 
                    src.Employee.Contracts.First(c => c.IsActive).Job.Department != null ? 
                    src.Employee.Contracts.First(c => c.IsActive).Job.Department.Name : null))
                .ReverseMap();

            // AttendanceRecord mappings
            CreateMap<AttendanceRecord, AttendanceRecordDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ReverseMap();

            // PayrollRecord mappings
            CreateMap<PayrollRecord, PayrollRecordDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ForMember(dest => dest.Components, opt => opt.MapFrom(src => src.Components))
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

            // EmployeeContract mappings
            CreateMap<EmployeeContract, EmployeeContractDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : null))
                .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => src.ContractType.ToString()))
                .ForMember(dest => dest.SalaryStructureName, opt => opt.MapFrom(src => src.SalaryStructure != null ? src.SalaryStructure.Name : null))
                .ReverseMap();

            // HrAttachment mappings
            CreateMap<HrAttachment, HrAttachmentDto>()
                .ReverseMap();

            // SalaryStructure mappings
            CreateMap<SalaryStructure, SalaryStructureDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap();

            // SalaryStructureComponent mappings
            CreateMap<SalaryStructureComponent, SalaryStructureComponentDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap();

            // SalaryStructurePaged mappings
            CreateMap<SalaryStructure, SalaryStructurePagedDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.ComponentCount, opt => opt.MapFrom(src => src.Components.Count))
                .ReverseMap();
        }
    }
}