using MediatR;
using System;
using Website.Application.Features.Testimonials.DTOs;

namespace Website.Application.Features.Testimonials.Queries.GetTestimonialById
{
    public class GetTestimonialByIdQuery : IRequest<TestimonialDto?>
    {
        public Guid Id { get; set; }
    }
}
