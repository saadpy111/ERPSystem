using System;

namespace Website.Application.Features.Testimonials.DTOs
{
    public class TestimonialDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int Order { get; set; }
        public bool IsVisible { get; set; }
    }
}
