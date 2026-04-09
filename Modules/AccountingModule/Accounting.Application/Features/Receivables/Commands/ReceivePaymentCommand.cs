using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Commands.PostTransaction;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;

namespace Accounting.Application.Features.Receivables.Commands
{
    public class ReceivePaymentCommand : IRequest<int>
    {
        public int ReceivableId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CashAccountId { get; set; }
        public string? Description { get; set; }
        public int CurrencyId { get; set; }
    }

    public class ReceivePaymentCommandHandler : IRequestHandler<ReceivePaymentCommand, int>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public ReceivePaymentCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<int> Handle(ReceivePaymentCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            var receivable = await _uow.Receivables.GetByIdAsync(request.ReceivableId);
            if (receivable == null)
                throw new ArgumentException("Receivable not found");

            if (request.Amount > receivable.RemainingAmount)
                throw new ArgumentException("Payment cannot exceed remaining amount");

            var cashAccount = await _uow.CashAccounts.GetByIdAsync(request.CashAccountId);
            if (cashAccount == null)
                throw new ArgumentException("CashAccount not found");

            var payment = new ReceivablePayment
            {
                ReceivableId = request.ReceivableId,
                Amount = request.Amount,
                Date = request.Date,
                CashAccountId = request.CashAccountId,
                Description = request.Description
            };

            await _uow.ReceivablePayments.AddAsync(payment);

            receivable.PaidAmount += request.Amount;
            receivable.RemainingAmount -= request.Amount;
            
            if (receivable.RemainingAmount == 0)
            {
                receivable.Status = ReceivableStatus.Paid;
            }
            else
            {
                receivable.Status = ReceivableStatus.PartiallyPaid;
            }
            
            _uow.Receivables.Update(receivable);
            await _uow.SaveChangesAsync();

            var postCommand = new PostTransactionCommand
            {
                SourceType = SourceType.ReceivablePayment,
                SourceId = payment.Id,
                Date = request.Date,
                Description = request.Description ?? $"Payment received for receivable {receivable.Id}",
                Reference = receivable.Reference,
                CurrencyId = request.CurrencyId
            };
            
            await _mediator.Send(postCommand, cancellationToken);

            return payment.Id;
        }
    }
}
