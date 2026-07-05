using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Domain.Entities;
using Subscription.Domain.Enums;
using Subscription.Persistence.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly SubscriptionDbContext _context;

        public PaymentRepository(SubscriptionDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(string id, CancellationToken ct = default)
        {
            return await _context.Payments
                .Include(p => p.Transactions)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<Payment?> FindPendingAsync(
            string tenantId, 
            PaymentPurpose purpose, 
            string targetId, 
            CancellationToken ct = default)
        {
            return await _context.Payments
                .Where(p => p.TenantId == tenantId &&
                            p.Purpose == purpose &&
                            p.TargetId == targetId &&
                            p.Status == PaymentStatus.Pending &&
                            (p.ExpiresAt == null || p.ExpiresAt > DateTime.UtcNow))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Payment?> FindPendingByUserAsync(
            string userId,
            PaymentPurpose purpose,
            string targetId,
            CancellationToken ct = default)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId &&
                            p.Purpose == purpose &&
                            p.TargetId == targetId &&
                            p.Status == PaymentStatus.Pending &&
                            (p.ExpiresAt == null || p.ExpiresAt > DateTime.UtcNow))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> HasTransactionAsync(
            string paymentId, 
            string gatewayTransactionId, 
            CancellationToken ct = default)
        {
            return await _context.PaymentTransactions
                .AnyAsync(pt => pt.PaymentId == paymentId && 
                                pt.GatewayTransactionId == gatewayTransactionId, ct);
        }

        public async Task<Payment> CreateAsync(Payment payment, CancellationToken ct = default)
        {
            await _context.Payments.AddAsync(payment, ct);
            return payment;
        }

        public Task UpdateAsync(Payment payment, CancellationToken ct = default)
        {
            _context.Payments.Update(payment);
            return Task.CompletedTask;
        }
    }
}
