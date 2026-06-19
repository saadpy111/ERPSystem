using System;

namespace Website.Domain.Entities
{
    public class Testimonial : BaseEntity
    {
        public string CustomerName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
        public bool IsVisible { get; set; } = false;
        public int Order { get; set; }
    }
}
