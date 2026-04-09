using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Commands.PostTransaction;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Payables.Commands
{
    public class PayPayableCommand : IRequest<Result<int>>
    {
        public int PayableId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CashAccountId { get; set; }
        public string? Description { get; set; }
        public int CurrencyId { get; set; }
    }

    public class PayPayableCommandHandler : IRequestHandler<PayPayableCommand, Result<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public PayPayableCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(PayPayableCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            var payable = await _uow.Payables.GetByIdAsync(request.PayableId);
            if (payable == null)
                throw new ArgumentException("Payable not found");

            if (request.Amount > payable.RemainingAmount)
                throw new ArgumentException("Payment cannot exceed remaining amount");

            var cashAccount = await _uow.CashAccounts.GetByIdAsync(request.CashAccountId);
            if (cashAccount == null)
                throw new ArgumentException("CashAccount not found");

            var payment = new PayablePayment
            {
                PayableId = request.PayableId,
                Amount = request.Amount,
                Date = request.Date,
                CashAccountId = request.CashAccountId,
                Description = request.Description
            };

            await _uow.PayablePayments.AddAsync(payment);

            payable.PaidAmount += request.Amount;
            payable.RemainingAmount -= request.Amount;
            
            if (payable.RemainingAmount == 0)
            {
                payable.Status = PayableStatus.Paid;
            }
            else
            {
                payable.Status = PayableStatus.PartiallyPaid;
            }
            
            _uow.Payables.Update(payable);
            await _uow.SaveChangesAsync();

            var postCommand = new PostTransactionCommand
            {
                SourceType = SourceType.PayablePayment,
                SourceId = payment.Id,
                Date = request.Date,
                Description = request.Description ?? $"Payment sent for payable {payable.Id}",
                Reference = payable.Reference,
                CurrencyId = request.CurrencyId
            };
            
            await _mediator.Send(postCommand, cancellationToken);

            return payment.Id;
        }
    }
}
