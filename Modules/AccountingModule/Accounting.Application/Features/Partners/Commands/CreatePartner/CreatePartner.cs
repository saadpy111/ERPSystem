using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Partners.Commands.CreatePartner
{
    public class CreatePartnerCommand : IRequest<Result<int>>
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public PartnerType Type { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? ContactPerson { get; set; }
        public string? PaymentTerms { get; set; }
        public decimal? CreditLimit { get; set; }
        public int DefaultCurrencyId { get; set; }
        public int? DefaultTaxId { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public string? PostalCode { get; set; }
    }

    public class CreatePartnerCommandValidator : AbstractValidator<CreatePartnerCommand>
    {
        public CreatePartnerCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.DefaultCurrencyId).NotEmpty();
        }
    }

    public class CreatePartnerCommandHandler : IRequestHandler<CreatePartnerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePartnerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreatePartnerCommand request, CancellationToken cancellationToken)
        {
            var partner = new Partner
            {
                Code = request.Code,
                NameAr = request.NameAr,
                NameEn = request.NameEn,
                Type = request.Type,
                Phone = request.Phone,
                Email = request.Email,
                TaxNumber = request.TaxNumber,
                ContactPerson = request.ContactPerson,
                PaymentTerms = request.PaymentTerms,
                CreditLimit = request.CreditLimit,
                DefaultCurrencyId = request.DefaultCurrencyId,
                DefaultTaxId = request.DefaultTaxId,
                StreetAddress = request.StreetAddress,
                City = request.City,
                Governorate = request.Governorate,
                PostalCode = request.PostalCode,
                IsActive = true
            };
            
            await _unitOfWork.Partners.AddAsync(partner);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Ok(partner.Id, "Partner created successfully.");
        }
    }
}
