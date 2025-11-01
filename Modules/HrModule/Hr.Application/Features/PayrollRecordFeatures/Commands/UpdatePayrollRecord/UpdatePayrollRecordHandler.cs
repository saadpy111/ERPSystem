using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.UpdatePayrollRecord
{
    public class UpdatePayrollRecordHandler : IRequestHandler<UpdatePayrollRecordRequest, UpdatePayrollRecordResponse>
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePayrollRecordHandler(IPayrollRecordRepository payrollRecordRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdatePayrollRecordResponse> Handle(UpdatePayrollRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payrollRecord = await _payrollRecordRepository.GetByIdAsync(request.PayrollId);
                if (payrollRecord == null)
                {
                    return new UpdatePayrollRecordResponse
                    {
                        Success = false,
                        Message = "Payroll record not found"
                    };
                }

                payrollRecord.EmployeeId = request.EmployeeId;
                payrollRecord.PeriodYear = request.PeriodYear;
                payrollRecord.PeriodMonth = request.PeriodMonth;
                payrollRecord.BaseSalary = request.BaseSalary;
                payrollRecord.TotalAllowances = request.TotalAllowances;
                payrollRecord.TotalDeductions = request.TotalDeductions;
                payrollRecord.TotalGrossSalary = request.TotalGrossSalary;
                payrollRecord.NetSalary = request.NetSalary;
                payrollRecord.Status = request.Status;

                _payrollRecordRepository.Update(payrollRecord);
                await _unitOfWork.SaveChangesAsync();

                var payrollRecordDto = _mapper.Map<DTOs.PayrollRecordDto>(payrollRecord);

                return new UpdatePayrollRecordResponse
                {
                    Success = true,
                    Message = "Payroll record updated successfully",
                    PayrollRecord = payrollRecordDto
                };
            }
            catch (Exception ex)
            {
                return new UpdatePayrollRecordResponse
                {
                    Success = false,
                    Message = $"Error updating payroll record: {ex.Message}"
                };
            }
        }
    }
}
