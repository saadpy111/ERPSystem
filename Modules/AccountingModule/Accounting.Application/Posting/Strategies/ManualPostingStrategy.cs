using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    public class ManualPostingStrategy : IPostingStrategy
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManualPostingStrategy(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Manual;

        public async Task<List<JournalEntryLine>> GenerateLinesAsync(IPostingRequest request)
        {
            var journalEntry = await _unitOfWork.JournalEntries.GetByIdWithLinesAsync(request.SourceId);
            if (journalEntry == null)
                throw new Exception($"Journal entry with ID {request.SourceId} not found.");

            return journalEntry.Lines.ToList();
        }
    }
}
