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
    public class CreateReceivableCommand : IRequest<Result<int>>
    {
        public int PartnerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public int CurrencyId { get; set; }
    }

    public class CreateReceivableCommandHandler : IRequestHandler<CreateReceivableCommand, Result<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public CreateReceivableCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(CreateReceivableCommand request, CancellationToken cancellationToken)
        {
            return await _uow.ExecuteTransactionAsync<Result<int>>(async () =>
            {
                var partner = await _uow.Partners.GetByIdAsync(request.PartnerId);
                if (partner == null)
                    return Result<int>.Failure("Partner not found.");

                var receivable = new Receivable
                {
                    PartnerId = request.PartnerId,
                    Amount = request.Amount,
                    RemainingAmount = request.Amount,
                    PaidAmount = 0,
                    DueDate = request.DueDate,
                    Status = ReceivableStatus.Open,
                    Reference = request.Reference,
                    Description = request.Description
                };

                await _uow.Receivables.AddAsync(receivable);

                await _uow.SaveChangesAsync();

                var postCommand = new PostTransactionCommand
                {
                    SourceType = SourceType.Receivable,
                    SourceId = receivable.Id,
                    Date = DateTime.UtcNow,
                    Description = request.Description ?? $"Receivable created for {partner.NameEn}",
                    Reference = request.Reference,
                    CurrencyId = request.CurrencyId
                };

                var postResult = await _mediator.Send(postCommand, cancellationToken);

                if (!postResult.Success)
                    return Result<int>.Failure(postResult.Message);

                await _uow.SaveChangesAsync();

                return Result<int>.Ok(receivable.Id, "Receivable created successfully");
            });
        }
    }
}
