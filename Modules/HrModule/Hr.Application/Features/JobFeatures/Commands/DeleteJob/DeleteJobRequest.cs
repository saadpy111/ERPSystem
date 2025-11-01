using MediatR;

namespace Hr.Application.Features.JobFeatures.DeleteJob
{
    public class DeleteJobRequest : IRequest<DeleteJobResponse>
    {
        public int Id { get; set; }
    }
}
