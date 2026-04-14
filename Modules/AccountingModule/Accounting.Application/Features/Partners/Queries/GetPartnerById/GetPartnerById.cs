using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using Accounting.Application.Common.Models;
using Accounting.Domain.Enums;

namespace Accounting.Application.Features.Partners.Queries.GetPartnerById
{
    public class PartnerDetailDto
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
        public string CurrencyName { get; set; } = null!;
        public int? DefaultTaxId { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetPartnerByIdQuery : IRequest<Result<PartnerDetailDto>>
    {
        public int Id { get; set; }
    }

    public class GetPartnerByIdQueryHandler : IRequestHandler<GetPartnerByIdQuery, Result<PartnerDetailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPartnerByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PartnerDetailDto>> Handle(GetPartnerByIdQuery request, CancellationToken cancellationToken)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(request.Id);

            if (partner == null)
                return Result<PartnerDetailDto>.Failure("Partner not found.");

            var dto = new PartnerDetailDto
            {
                Id = partner.Id,
                Code = partner.Code,
                NameAr = partner.NameAr,
                NameEn = partner.NameEn,
                Type = partner.Type,
                Phone = partner.Phone,
                Email = partner.Email,
                TaxNumber = partner.TaxNumber,
                ContactPerson = partner.ContactPerson,
                PaymentTerms = partner.PaymentTerms,
                CreditLimit = partner.CreditLimit,
                DefaultCurrencyId = partner.DefaultCurrencyId,
                CurrencyName = partner.DefaultCurrency?.NameEn ?? "N/A",
                DefaultTaxId = partner.DefaultTaxId,
                StreetAddress = partner.StreetAddress,
                City = partner.City,
                Governorate = partner.Governorate,
                PostalCode = partner.PostalCode,
                IsActive = partner.IsActive
            };

            return Result<PartnerDetailDto>.Ok(dto);
        }
    }
}
