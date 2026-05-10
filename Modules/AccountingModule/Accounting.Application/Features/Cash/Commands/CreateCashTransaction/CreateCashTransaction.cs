using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Posting.Commands.PostTransaction;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Cash.Commands.CreateCashTransaction
{
    public class CreateCashTransactionCommand : IRequest<Result<int>>
    {
        public int CashAccountId { get; set; }
        public CashTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Reference { get; set; }
        public int CurrencyId { get; set; }
        public int? PartnerId { get; set; }
        public int OffsetAccountId { get; set; }

        /// <summary>
        /// The actor creating this cash transaction — stored for audit trail.
        /// </summary>
        public string? CreatedByUser { get; set; }
    }

    public class CreateCashTransactionCommandValidator : AbstractValidator<CreateCashTransactionCommand>
    {
        public CreateCashTransactionCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.CashAccountId).GreaterThan(0);
            RuleFor(x => x.OffsetAccountId).GreaterThan(0);
            RuleFor(x => x.CurrencyId).GreaterThan(0);
        }
    }

    public class CreateCashTransactionCommandHandler : IRequestHandler<CreateCashTransactionCommand, Result<int>>
    {
        private readonly IAccountingDbContext _context;
        private readonly IMediator _mediator;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IAccountBalanceService _balanceService;

        public CreateCashTransactionCommandHandler(
            IAccountingDbContext context,
            IMediator mediator,
            IExchangeRateService exchangeRateService,
            IAccountBalanceService balanceService)
        {
            _context = context;
            _mediator = mediator;
            _exchangeRateService = exchangeRateService;
            _balanceService = balanceService;
        }

        public async Task<Result<int>> Handle(
            CreateCashTransactionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var cashAccount = await _context.CashAccounts
                    .FirstOrDefaultAsync(c => c.Id == request.CashAccountId, cancellationToken);

                if (cashAccount == null)
                    return Result<int>.Failure("Cash Account not found.");

                var rate = await _exchangeRateService.GetRateAsync(request.CurrencyId, request.Date);

                decimal baseAmount = request.Amount * rate;

                if (request.Type == CashTransactionType.Payment)
                {
                    var balanceResult = await _balanceService.ValidateSufficientBalanceAsync(
                        glAccountId:           cashAccount.AccountId,
                        allowNegativeBalance:  cashAccount.AllowNegativeBalance,
                        requiredBaseAmount:    baseAmount,   
                        cancellationToken:     cancellationToken);

                    if (!balanceResult.Success)
                        return Result<int>.Failure(balanceResult.Message);
                }

                var transaction = new CashTransaction
                {
                    CashAccountId   = request.CashAccountId,
                    Type            = request.Type,
                    Amount          = request.Amount,
                    Date            = request.Date,
                    Description     = request.Description,
                    Reference       = request.Reference,
                    CurrencyId      = request.CurrencyId,
                    ExchangeRate    = rate,
                    BaseAmount      = baseAmount,
                    PartnerId       = request.PartnerId,
                    OffsetAccountId = request.OffsetAccountId
                };

                await _context.CashTransactions.AddAsync(transaction, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var postCommand = new PostTransactionCommand
                {
                    SourceType  = request.Type == CashTransactionType.Receipt
                                      ? SourceType.CashReceipt
                                      : SourceType.CashPayment,
                    SourceId    = transaction.Id,
                    Date        = transaction.Date,
                    Description = transaction.Description,
                    CurrencyId  = transaction.CurrencyId,
                    PostedBy    = request.CreatedByUser
                };

                await _mediator.Send(postCommand, cancellationToken);

                return Result<int>.Ok(transaction.Id, "Cash transaction created and posted successfully.");
            }
            catch (BusinessException ex)
            {
                return Result<int>.Failure(ex.Message);
            }
        }
    }
}
