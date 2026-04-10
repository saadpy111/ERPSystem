using Accounting.Application.Common.Models;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Interfaces
{
    public interface IPostingService
    {
        Task<Result<int>> PostAsync(IPostingRequest request);
    }
}
