using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.GetAllPayrollRecords
{
    public class GetAllPayrollRecordsHandler : IRequestHandler<GetAllPayrollRecordsRequest, GetAllPayrollRecordsResponse>
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IMapper _mapper;

        public GetAllPayrollRecordsHandler(IPayrollRecordRepository payrollRecordRepository, IMapper mapper)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _mapper = mapper;
        }

        public async Task<GetAllPayrollRecordsResponse> Handle(GetAllPayrollRecordsRequest request, CancellationToken cancellationToken)
        {
            var payrollRecords = await _payrollRecordRepository.GetAllAsync();
            var payrollRecordDtos = _mapper.Map<IEnumerable<PayrollRecordDto>>(payrollRecords);

            return new GetAllPayrollRecordsResponse
            {
                PayrollRecords = payrollRecordDtos
            };
        }
    }
}
