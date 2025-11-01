using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeSalaryDetails
{
    public class GetEmployeeSalaryDetailsRequest : IRequest<GetEmployeeSalaryDetailsResponse>
    {
        public int EmployeeId { get; set; }
    }
}
