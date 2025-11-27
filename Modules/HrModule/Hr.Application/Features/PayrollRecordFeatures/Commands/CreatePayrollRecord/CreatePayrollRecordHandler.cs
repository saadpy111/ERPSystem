using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Features.PayrollRecordFeatures.Commands.CalculatePayroll;
using Hr.Application.Features.PayrollRecordFeatures.Commands.RecalculatePayroll;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.PayrollRecordFeatures.CreatePayrollRecord
{
    public class CreatePayrollRecordHandler : IRequestHandler<CreatePayrollRecordRequest, CreatePayrollRecordResponse>
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreatePayrollRecordHandler(
            IPayrollRecordRepository payrollRecordRepository,
            IEmployeeContractRepository employeeContractRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMediator mediator)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _employeeContractRepository = employeeContractRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<CreatePayrollRecordResponse> Handle(CreatePayrollRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = await _employeeContractRepository.GetContractByEmployeeIdAsync(request.EmployeeId);

                if (contract == null)
                    throw new Exception("لم يتم العثور على عقد نشط.");

                var payroll = new PayrollRecord
                {
                    EmployeeId = request.EmployeeId,
                    PeriodYear = request.PeriodYear,
                    PeriodMonth = request.PeriodMonth,
                    BaseSalary = contract.Salary,
                    Status = PayrollStatus.Draft,
                    
                };
         
                foreach (var comp in contract.SalaryStructure?.Components ?? new List<SalaryStructureComponent>())
                {
                  //  decimal amount = comp.FixedAmount ?? (contract.Salary * (comp.Percentage ?? 0) / 100);

                    var pc = new PayrollComponent
                    {
                        Name = comp.Name,
                        ComponentType = comp.Type,
                         FixedAmount = comp.FixedAmount,
                          Percentage = comp.Percentage
                    };

                    payroll.Components.Add(pc);
                }

          

                await _payrollRecordRepository.AddAsync(payroll);
                await _unitOfWork.SaveChangesAsync();

                // Use the calculate handler to calculate payroll components
                var recalculateRequest = new CalculatePayrollRequest
                {
                    PayrollRecordId = payroll.PayrollId
                };


                var recalculateResult = await _mediator.Send(recalculateRequest, cancellationToken);

                if (!recalculateResult.Success)
                {
                    return new CreatePayrollRecordResponse
                    {
                        Success = false,
                        Message = recalculateResult.Message
                    };
                }

                // Refresh the payroll record to get updated values
                var updatedPayroll = await _payrollRecordRepository.GetByIdAsync(payroll.PayrollId);
                var payrollRecordDto = _mapper.Map<DTOs.PayrollRecordDto>(updatedPayroll);

                return new CreatePayrollRecordResponse
                {
                    Success = true,
                    Message = "تم إنشاء سجل الرواتب بنجاح",
                    PayrollRecord = payrollRecordDto
                };
            }
            catch (Exception ex)
            {
                return new CreatePayrollRecordResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إنشاء سجل الرواتب"
                };
            }
        }
    }
}