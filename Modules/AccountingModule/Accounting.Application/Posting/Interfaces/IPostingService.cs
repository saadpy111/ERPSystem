using System.Threading.Tasks;

namespace Accounting.Application.Posting.Interfaces
{
    public interface IPostingService
    {
        Task<int> PostAsync(IPostingRequest request);
    }
}
