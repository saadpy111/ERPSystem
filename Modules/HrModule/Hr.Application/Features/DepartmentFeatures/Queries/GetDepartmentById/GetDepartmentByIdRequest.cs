using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.GetDepartmentById
{
    public class GetDepartmentByIdRequest : IRequest<GetDepartmentByIdResponse>
    {
        public int Id { get; set; }
    }
}
