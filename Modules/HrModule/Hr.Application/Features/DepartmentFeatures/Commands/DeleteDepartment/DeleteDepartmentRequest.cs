using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.DeleteDepartment
{
    public class DeleteDepartmentRequest : IRequest<DeleteDepartmentResponse>
    {
        public int Id { get; set; }
    }
}
