using MediatR;
using System;

namespace Website.Application.Features.Testimonials.Commands.UpdateTestimonial
{
    public class UpdateTestimonialCommand : IRequest<TestimonialCommandResponse>
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
