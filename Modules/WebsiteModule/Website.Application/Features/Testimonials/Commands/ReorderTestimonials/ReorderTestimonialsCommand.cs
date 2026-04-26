using MediatR;
using System;
using System.Collections.Generic;

namespace Website.Application.Features.Testimonials.Commands.ReorderTestimonials
{
    public class ReorderTestimonialsCommand : IRequest<TestimonialCommandResponse>
    {
        public List<TestimonialOrderDto> OrderedItems { get; set; } = new();
    }

    public class TestimonialOrderDto
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
    }
}
