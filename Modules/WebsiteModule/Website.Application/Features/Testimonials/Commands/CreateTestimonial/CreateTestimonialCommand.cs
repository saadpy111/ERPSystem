using MediatR;

namespace Website.Application.Features.Testimonials.Commands.CreateTestimonial
{
    public class CreateTestimonialCommand : IRequest<TestimonialCommandResponse>
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
