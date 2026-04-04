using System;

namespace Accounting.Domain.Entities
{
    public class Sequence
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Prefix { get; set; } = null!;
        public int CurrentNumber { get; set; }
        public int Year { get; set; }
        public int TenantId { get; set; }
    }
}
