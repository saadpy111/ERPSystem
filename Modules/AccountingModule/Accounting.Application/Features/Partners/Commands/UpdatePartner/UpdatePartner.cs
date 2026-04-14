using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Partners.Commands.UpdatePartner
{
    public class UpdatePartnerCommand : IRequest<Result>
    {
        public int Id { get; set; }
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
        public bool IsActive { get; set; }
    }

    public class UpdatePartnerCommandValidator : AbstractValidator<UpdatePartnerCommand>
    {
        public UpdatePartnerCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.DefaultCurrencyId).NotEmpty();
        }
    }

    public class UpdatePartnerCommandHandler : IRequestHandler<UpdatePartnerCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePartnerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdatePartnerCommand request, CancellationToken cancellationToken)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(request.Id);
            if (partner == null)
                return Result.Failure("Partner not found.");

            partner.Code = request.Code;
            partner.NameAr = request.NameAr;
            partner.NameEn = request.NameEn;
            partner.Type = request.Type;
            partner.Phone = request.Phone;
            partner.Email = request.Email;
            partner.TaxNumber = request.TaxNumber;
            partner.ContactPerson = request.ContactPerson;
            partner.PaymentTerms = request.PaymentTerms;
            partner.CreditLimit = request.CreditLimit;
            partner.DefaultCurrencyId = request.DefaultCurrencyId;
            partner.DefaultTaxId = request.DefaultTaxId;
            partner.StreetAddress = request.StreetAddress;
            partner.City = request.City;
            partner.Governorate = request.Governorate;
            partner.PostalCode = request.PostalCode;
            partner.IsActive = request.IsActive;

            _unitOfWork.Partners.Update(partner);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok("Partner updated successfully.");
        }
    }
}
