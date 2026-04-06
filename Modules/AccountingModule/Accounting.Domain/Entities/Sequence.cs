using Accounting.Domain.Common;

namespace Accounting.Domain.Entities
{
    public class Sequence : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Prefix { get; set; } = null!;
        public int CurrentNumber { get; set; }
        public int Year { get; set; }
    }
}
