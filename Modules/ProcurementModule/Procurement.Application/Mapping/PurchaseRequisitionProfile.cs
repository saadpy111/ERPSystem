using AutoMapper;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;

namespace Procurement.Application.Mapping
{
    public class PurchaseRequisitionProfile : Profile
    {
        public PurchaseRequisitionProfile()
        {
            CreateMap<PurchaseRequisition, PurchaseRequisitionDto>();
            CreateMap<CreatePurchaseRequisitionDto, PurchaseRequisition>();
        }
    }
}