using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordById
{
    public class GetPayrollRecordByIdHandler : IRequestHandler<GetPayrollRecordByIdRequest, GetPayrollRecordByIdResponse>
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IMapper _mapper;

        public GetPayrollRecordByIdHandler(IPayrollRecordRepository payrollRecordRepository, IMapper mapper)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _mapper = mapper;
        }

        public async Task<GetPayrollRecordByIdResponse> Handle(GetPayrollRecordByIdRequest request, CancellationToken cancellationToken)
        {
            var payrollRecord = await _payrollRecordRepository.GetByIdAsync(request.Id);
            var payrollRecordDto = _mapper.Map<DTOs.PayrollRecordDto>(payrollRecord);

            return new GetPayrollRecordByIdResponse
            {
                PayrollRecord = payrollRecordDto
            };
        }
    }
}
