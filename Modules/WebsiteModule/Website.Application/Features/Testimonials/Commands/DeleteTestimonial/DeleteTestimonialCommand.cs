using MediatR;
using System;

namespace Website.Application.Features.Testimonials.Commands.DeleteTestimonial
{
    public class DeleteTestimonialCommand : IRequest<TestimonialCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
