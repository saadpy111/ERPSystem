using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Commands.PostTransaction;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Receivables.Commands
{
    public class ReceivePaymentCommand : IRequest<Result<int>>
    {
        public int ReceivableId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CashAccountId { get; set; }
        public string? Description { get; set; }
        public int CurrencyId { get; set; }
    }

    public class ReceivePaymentCommandHandler : IRequestHandler<ReceivePaymentCommand, Result<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public ReceivePaymentCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(ReceivePaymentCommand request, CancellationToken cancellationToken)
        {
            return await _uow.ExecuteTransactionAsync<Result<int>>(async () =>
            {
                var receivable = await _uow.Receivables.GetByIdAsync(request.ReceivableId);
                if (receivable == null)
                    return Result<int>.Failure("Receivable not found.");

                var cashAccount = await _uow.CashAccounts.GetByIdAsync(request.CashAccountId);
                if (cashAccount == null)
                    return Result<int>.Failure("Cash account not found.");

                if (request.Amount <= 0)
                    return Result<int>.Failure("Invalid payment amount.");

                if (request.Amount > receivable.RemainingAmount)
                    return Result<int>.Failure("Payment exceeds remaining amount.");

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
                    receivable.Status = ReceivableStatus.Paid;
                else
                    receivable.Status = ReceivableStatus.PartiallyPaid;

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

                var postResult = await _mediator.Send(postCommand, cancellationToken);

                if (!postResult.Success)
                    return Result<int>.Failure(postResult.Message);

                await _uow.SaveChangesAsync();

                return Result<int>.Ok(payment.Id, "Payment received successfully");
            });
        }
    }

}
