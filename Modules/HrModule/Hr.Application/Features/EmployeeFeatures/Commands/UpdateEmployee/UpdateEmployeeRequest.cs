using MediatR;
using Hr.Domain.Enums;

namespace Hr.Application.Features.EmployeeFeatures.UpdateEmployee
{
    public class UpdateEmployeeRequest : IRequest<UpdateEmployeeResponse>
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string? Position { get; set; }
        public DateTime HiringDate { get; set; }
        public decimal? Salary { get; set; }
        public EmployeeStatus Status { get; set; }
    }
}
