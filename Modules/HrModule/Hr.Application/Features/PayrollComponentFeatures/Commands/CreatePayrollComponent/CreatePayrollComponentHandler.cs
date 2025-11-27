using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Features.PayrollRecordFeatures.Commands.CalculatePayroll;
using Hr.Application.Features.PayrollRecordFeatures.Commands.RecalculatePayroll;
using Hr.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent
{
    public class CreatePayrollComponentHandler : IRequestHandler<CreatePayrollComponentRequest, CreatePayrollComponentResponse>
    {
        private readonly IPayrollComponentRepository _payrollComponentRepository;
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreatePayrollComponentHandler(
            IPayrollComponentRepository payrollComponentRepository,
            IPayrollRecordRepository payrollRecordRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMediator mediator)
        {
            _payrollComponentRepository = payrollComponentRepository;
            _payrollRecordRepository = payrollRecordRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<CreatePayrollComponentResponse> Handle(CreatePayrollComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payrollComponent = new PayrollComponent
                {
                    PayrollRecordId = request.PayrollRecordId,
                    Name = request.Name,
                    ComponentType = request.ComponentType,
                    Percentage = request.Percentage,
                     FixedAmount = request.FixedAmount
                };

                await _payrollComponentRepository.AddAsync(payrollComponent);
                await _unitOfWork.SaveChangesAsync();

                // Recalculate the entire payroll record when a new component is added
                var recalculateRequest = new CalculatePayrollRequest
                {
                    PayrollRecordId = request.PayrollRecordId
                };
                var recalculateResult = await _mediator.Send(recalculateRequest, cancellationToken);

                if (!recalculateResult.Success)
                {
                    return new CreatePayrollComponentResponse
                    {
                        Success = false,
                        Message = recalculateResult.Message
                    };
                }

                var payrollComponentDto = _mapper.Map<DTOs.PayrollComponentDto>(payrollComponent);

                return new CreatePayrollComponentResponse
                {
                    Success = true,
                    Message = "تم إنشاء مكون الرواتب بنجاح",
                    PayrollComponent = payrollComponentDto
                };
            }
            catch (Exception ex)
            {
                return new CreatePayrollComponentResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إنشاء مكون الرواتب"
                };
            }
        }
    }
}