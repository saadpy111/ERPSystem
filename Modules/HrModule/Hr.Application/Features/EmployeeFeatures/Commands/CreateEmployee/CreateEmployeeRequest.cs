using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.CreateEmployee
{
    public class CreateEmployeeRequest : IRequest<CreateEmployeeResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public DateTime HiringDate { get; set; }
        public decimal BaseSalary { get; set; }
    }
}
