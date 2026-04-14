using Accounting.Application.Common.Models;
using Accounting.Domain.Entities;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Interfaces
{
    public interface IPostingService
    {
        Task<Result<JournalEntry>> PostAsync(IPostingRequest request);
    }
}
