using AutoMapper;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;

namespace Procurement.Application.Mapping
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<Vendor, VendorDto>();
            CreateMap<CreateVendorDto, Vendor>();
            CreateMap<UpdateVendorDto, Vendor>();
        }
    }
}