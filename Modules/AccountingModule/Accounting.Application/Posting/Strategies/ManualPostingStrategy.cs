using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
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
            var journalEntry = await _unitOfWork.JournalEntries.GetByIdAsync(request.SourceId);
            return journalEntry?.Lines.ToList() ?? new List<JournalEntryLine>();
        }
    }
}
