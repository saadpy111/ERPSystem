using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Posting.Commands.PostTransaction;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Cash.Commands.CreateCashTransaction
{
    public class CreateCashTransactionCommand : IRequest<int>
    {
        public int CashAccountId { get; set; }
        public CashTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Reference { get; set; }
        public int? PartnerId { get; set; }
        public int OffsetAccountId { get; set; }
    }

    public class CreateCashTransactionCommandValidator : AbstractValidator<CreateCashTransactionCommand>
    {
        public CreateCashTransactionCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.CashAccountId).GreaterThan(0);
            RuleFor(x => x.OffsetAccountId).GreaterThan(0);
        }
    }

    public class CreateCashTransactionCommandHandler : IRequestHandler<CreateCashTransactionCommand, int>
    {
        private readonly IAccountingDbContext _context;
        private readonly IMediator _mediator;

        public CreateCashTransactionCommandHandler(IAccountingDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateCashTransactionCommand request, CancellationToken cancellationToken)
        {
            var cashAccount = await _context.CashAccounts.FirstOrDefaultAsync(c => c.Id == request.CashAccountId, cancellationToken);
            if (cashAccount == null) throw new BusinessException("Cash Account not found.");

            // Calculate current balance if Payment to prevent negative balance
            if (request.Type == CashTransactionType.Payment)
            {
                var receipts = await _context.CashTransactions.Where(t => t.CashAccountId == request.CashAccountId && t.Type == CashTransactionType.Receipt).SumAsync(t => t.Amount, cancellationToken);
                var payments = await _context.CashTransactions.Where(t => t.CashAccountId == request.CashAccountId && t.Type == CashTransactionType.Payment).SumAsync(t => t.Amount, cancellationToken);
                var balance = receipts - payments;

                if (balance < request.Amount)
                {
                    throw new BusinessException("Insufficient balance in Cash Account.");
                }
            }

            var transaction = new CashTransaction
            {
                CashAccountId = request.CashAccountId,
                Type = request.Type,
                Amount = request.Amount,
                Date = request.Date,
                Description = request.Description,
                Reference = request.Reference,
                PartnerId = request.PartnerId,
                OffsetAccountId = request.OffsetAccountId,
                TenantId = cashAccount.TenantId
            };

            await _context.CashTransactions.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Trigger Posting Engine
            var postCommand = new PostTransactionCommand
            {
                SourceType = request.Type == CashTransactionType.Receipt ? SourceType.CashReceipt : SourceType.CashPayment,
                SourceId = transaction.Id,
                Date = transaction.Date,
                Description = transaction.Description
            };

            await _mediator.Send(postCommand, cancellationToken);

            return transaction.Id;
        }
    }
}
