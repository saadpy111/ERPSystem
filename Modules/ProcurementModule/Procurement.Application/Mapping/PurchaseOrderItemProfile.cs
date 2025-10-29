using AutoMapper;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;

namespace Procurement.Application.Mapping
{
    public class PurchaseOrderItemProfile : Profile
    {
        public PurchaseOrderItemProfile()
        {
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>();
        }
    }
}