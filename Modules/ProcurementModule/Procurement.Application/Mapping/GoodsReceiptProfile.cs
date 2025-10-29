using AutoMapper;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;

namespace Procurement.Application.Mapping
{
    public class GoodsReceiptProfile : Profile
    {
        public GoodsReceiptProfile()
        {
            CreateMap<GoodsReceipt, GoodsReceiptDto>();
            CreateMap<CreateGoodsReceiptDto, GoodsReceipt>();
        }
    }
}