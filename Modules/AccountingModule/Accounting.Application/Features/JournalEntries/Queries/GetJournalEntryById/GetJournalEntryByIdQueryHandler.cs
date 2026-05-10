using Accounting.Application.Features.JournalEntries.DTOs;
using Accounting.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.JournalEntries.Queries.GetJournalEntryById
{
    public class GetJournalEntryByIdQueryHandler : IRequestHandler<GetJournalEntryByIdQuery, Result<JournalEntryResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetJournalEntryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<JournalEntryResponseDto>> Handle(GetJournalEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var journalEntry = await _unitOfWork.JournalEntries.GetByIdAsync(request.Id);
            
            if (journalEntry == null)
            {
                return Result<JournalEntryResponseDto>.Failure($"Journal entry with ID {request.Id} not found.");
            }

            return _mapper.Map<JournalEntryResponseDto>(journalEntry);
        }
    }
}
