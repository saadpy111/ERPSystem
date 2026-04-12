using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using Accounting.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Interfaces
{
    public interface IPostingStrategy
    {
        bool CanHandle(SourceType type);
        Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest source);
    }
}
