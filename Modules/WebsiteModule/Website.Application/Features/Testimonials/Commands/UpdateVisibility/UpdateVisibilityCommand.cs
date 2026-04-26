using MediatR;
using System;

namespace Website.Application.Features.Testimonials.Commands.UpdateVisibility
{
    public class UpdateVisibilityCommand : IRequest<TestimonialCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
