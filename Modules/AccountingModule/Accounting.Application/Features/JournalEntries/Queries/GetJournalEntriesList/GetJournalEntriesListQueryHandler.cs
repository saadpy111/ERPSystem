using Accounting.Application.Common.Models;
using Accounting.Application.Features.JournalEntries.DTOs;
using Accounting.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.JournalEntries.Queries.GetJournalEntriesList
{
    public class GetJournalEntriesListQueryHandler : IRequestHandler<GetJournalEntriesListQuery, PagedResult<JournalEntryResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetJournalEntriesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<JournalEntryResponseDto>> Handle(GetJournalEntriesListQuery request, CancellationToken cancellationToken)
        {
            // Execute filter safely returning matches
            var entries = await _unitOfWork.JournalEntries.FindAsync(j => 
                (!request.StartDate.HasValue || j.Date >= request.StartDate.Value) &&
                (!request.EndDate.HasValue || j.Date <= request.EndDate.Value));

            var orderedList = entries.OrderByDescending(j => j.Date).ToList();

            var totalCount = orderedList.Count;

            var pagedData = orderedList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mappedData = _mapper.Map<List<JournalEntryResponseDto>>(pagedData);

            return new PagedResult<JournalEntryResponseDto>
            {
                Data = mappedData,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
