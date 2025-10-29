using AutoMapper;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;

namespace Procurement.Application.Mapping
{
    public class PurchaseInvoiceProfile : Profile
    {
        public PurchaseInvoiceProfile()
        {
            CreateMap<PurchaseInvoice, PurchaseInvoiceDto>();
            CreateMap<CreatePurchaseInvoiceDto, PurchaseInvoice>();
        }
    }
}