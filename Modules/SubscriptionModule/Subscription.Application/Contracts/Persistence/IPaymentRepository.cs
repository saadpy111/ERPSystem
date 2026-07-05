using Subscription.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Contracts.Persistence
{
    public interface IPaymentRepository
    {
        Task<Subscription.Domain.Entities.Payment?> GetByIdAsync(string id, CancellationToken ct = default);
        
        Task<Subscription.Domain.Entities.Payment?> FindPendingAsync(
            string tenantId, 
            PaymentPurpose purpose, 
            string targetId, 
            CancellationToken ct = default);

        Task<Subscription.Domain.Entities.Payment?> FindPendingByUserAsync(
            string userId,
            PaymentPurpose purpose,
            string targetId,
            CancellationToken ct = default);

        Task<bool> HasTransactionAsync(
            string paymentId, 
            string gatewayTransactionId, 
            CancellationToken ct = default);

        Task<Subscription.Domain.Entities.Payment> CreateAsync(Subscription.Domain.Entities.Payment payment, CancellationToken ct = default);
        
        Task UpdateAsync(Subscription.Domain.Entities.Payment payment, CancellationToken ct = default);
    }
}
