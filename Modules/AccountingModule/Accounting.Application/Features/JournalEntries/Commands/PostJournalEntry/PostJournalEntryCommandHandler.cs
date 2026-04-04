using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Posting.Requests;
using Accounting.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.JournalEntries.Commands.PostJournalEntry
{
    public class PostJournalEntryCommandHandler : IRequestHandler<PostJournalEntryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostingService _postingService;

        public PostJournalEntryCommandHandler(IUnitOfWork unitOfWork, IPostingService postingService)
        {
            _unitOfWork = unitOfWork;
            _postingService = postingService;
        }

        public async Task<int> Handle(PostJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var journalEntry = await _unitOfWork.JournalEntries.GetByIdAsync(request.JournalEntryId);
            
            if (journalEntry == null)
            {
                throw new Exception($"Journal entry {request.JournalEntryId} not found.");
            }

            if (journalEntry.Status == JournalStatus.Posted)
            {
                throw new Exception($"Journal entry is already posted.");
            }

            var postingRequest = new PostingRequest
            {
                SourceType = SourceType.Manual,
                SourceId = journalEntry.Id,
                CurrencyId = journalEntry.CurrencyId,
                Date = journalEntry.Date,
                Description = journalEntry.Description,
                Reference = journalEntry.Reference
            };

            return await _postingService.PostAsync(postingRequest);
        }
    }
}
