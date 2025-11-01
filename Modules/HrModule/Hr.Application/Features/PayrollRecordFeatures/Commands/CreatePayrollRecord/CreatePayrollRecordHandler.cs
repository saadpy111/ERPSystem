using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.CreatePayrollRecord
{
    public class CreatePayrollRecordHandler : IRequestHandler<CreatePayrollRecordRequest, CreatePayrollRecordResponse>
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePayrollRecordHandler(IPayrollRecordRepository payrollRecordRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreatePayrollRecordResponse> Handle(CreatePayrollRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var totalGrossSalary = request.BaseSalary + request.TotalAllowances;
                var netSalary = totalGrossSalary - request.TotalDeductions;

                var payrollRecord = new PayrollRecord
                {
                    EmployeeId = request.EmployeeId,
                    PeriodYear = request.PeriodYear,
                    PeriodMonth = request.PeriodMonth,
                    BaseSalary = request.BaseSalary,
                    TotalAllowances = request.TotalAllowances,
                    TotalDeductions = request.TotalDeductions,
                    TotalGrossSalary = totalGrossSalary,
                    NetSalary = netSalary,
                    Status = PayrollStatus.Draft
                };

                await _payrollRecordRepository.AddAsync(payrollRecord);
                await _unitOfWork.SaveChangesAsync();

                var payrollRecordDto = _mapper.Map<DTOs.PayrollRecordDto>(payrollRecord);

                return new CreatePayrollRecordResponse
                {
                    Success = true,
                    Message = "Payroll record created successfully",
                    PayrollRecord = payrollRecordDto
                };
            }
            catch (Exception ex)
            {
                return new CreatePayrollRecordResponse
                {
                    Success = false,
                    Message = $"Error creating payroll record: {ex.Message}"
                };
            }
        }
    }
}
