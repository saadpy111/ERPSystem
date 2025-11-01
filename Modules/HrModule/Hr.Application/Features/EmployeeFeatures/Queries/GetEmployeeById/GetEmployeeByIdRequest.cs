using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeById
{
    public class GetEmployeeByIdRequest : IRequest<GetEmployeeByIdResponse>
    {
        public int Id { get; set; }
    }
}
