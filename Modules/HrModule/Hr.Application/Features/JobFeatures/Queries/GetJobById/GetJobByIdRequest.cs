using MediatR;

namespace Hr.Application.Features.JobFeatures.GetJobById
{
    public class GetJobByIdRequest : IRequest<GetJobByIdResponse>
    {
        public int Id { get; set; }
    }
}
