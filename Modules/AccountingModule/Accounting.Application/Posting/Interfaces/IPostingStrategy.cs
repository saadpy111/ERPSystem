using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Interfaces
{
    public interface IPostingStrategy
    {
        bool CanHandle(SourceType type);
        Task<List<JournalEntryLine>> GenerateLinesAsync(IPostingRequest source);
    }
}
