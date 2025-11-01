using MediatR;

namespace Hr.Application.Features.JobFeatures.GetJobsByDepartment
{
    public class GetJobsByDepartmentRequest : IRequest<GetJobsByDepartmentResponse>
    {
        public int DepartmentId { get; set; }
    }
}
