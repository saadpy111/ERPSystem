using MediatR;
using System;

namespace Website.Application.Features.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommand : IRequest<DeleteBrandCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
