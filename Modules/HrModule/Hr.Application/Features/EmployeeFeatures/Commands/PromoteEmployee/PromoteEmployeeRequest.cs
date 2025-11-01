using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.PromoteEmployee
{
    public class PromoteEmployeeRequest : IRequest<PromoteEmployeeResponse>
    {
        public int EmployeeId { get; set; }
        public string NewJobTitle { get; set; } = string.Empty;
        public decimal NewBaseSalary { get; set; }
        public int? NewDepartmentId { get; set; }
    }
}
