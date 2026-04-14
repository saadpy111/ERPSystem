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
            return await _uow.ExecuteTransactionAsync<Result<int>>(async () =>
            {
                var payable = await _uow.Payables.GetByIdAsync(request.PayableId);
                if (payable == null)
                    return Result<int>.Failure("Payable not found.");

                if (request.Amount <= 0)
                    return Result<int>.Failure("Invalid payment amount.");

                if (request.Amount > payable.RemainingAmount)
                    return Result<int>.Failure("Payment exceeds remaining amount.");

                var cashAccount = await _uow.CashAccounts.GetByIdAsync(request.CashAccountId);
                if (cashAccount == null)
                    return Result<int>.Failure("Cash account not found.");

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
                    payable.Status = PayableStatus.Paid;
                else
                    payable.Status = PayableStatus.PartiallyPaid;

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

                var postResult = await _mediator.Send(postCommand, cancellationToken);

                if (!postResult.Success)
                    return Result<int>.Failure(postResult.Message);

                await _uow.SaveChangesAsync();

                return Result<int>.Ok(payment.Id, "Payment sent successfully");
            });
        }
    }
}
