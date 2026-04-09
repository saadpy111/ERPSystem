using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Commands.PostTransaction;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;

namespace Accounting.Application.Features.Payables.Commands
{
    public class CreatePayableCommand : IRequest<int>
    {
        public int PartnerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public int CurrencyId { get; set; }
    }

    public class CreatePayableCommandHandler : IRequestHandler<CreatePayableCommand, int>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public CreatePayableCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreatePayableCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            var partner = await _uow.Partners.GetByIdAsync(request.PartnerId);
            if (partner == null)
                throw new ArgumentException("Partner not found");

            var payable = new Payable
            {
                PartnerId = request.PartnerId,
                Amount = request.Amount,
                RemainingAmount = request.Amount,
                PaidAmount = 0,
                DueDate = request.DueDate,
                Status = PayableStatus.Open,
                Reference = request.Reference,
                Description = request.Description
            };

            await _uow.Payables.AddAsync(payable);
            await _uow.SaveChangesAsync();

            var postCommand = new PostTransactionCommand
            {
                SourceType = SourceType.Payable,
                SourceId = payable.Id,
                Date = DateTime.UtcNow,
                Description = request.Description ?? $"Payable created for {partner.NameEn}",
                Reference = request.Reference,
                CurrencyId = request.CurrencyId
            };
            
            await _mediator.Send(postCommand, cancellationToken);

            return payable.Id;
        }
    }
}
