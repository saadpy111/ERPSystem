using Accounting.Application.Features.JournalEntries.DTOs;
using Accounting.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Accounting.Application.Features.JournalEntries.Queries.GetJournalEntryById
{
    public class GetJournalEntryByIdQueryHandler : IRequestHandler<GetJournalEntryByIdQuery, JournalEntryResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetJournalEntryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<JournalEntryResponseDto> Handle(GetJournalEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var journalEntry = await _unitOfWork.JournalEntries.GetByIdAsync(request.Id);
            
            if (journalEntry == null)
            {
                throw new Exception($"Journal entry with ID {request.Id} not found.");
            }

            return _mapper.Map<JournalEntryResponseDto>(journalEntry);
        }
    }
}
