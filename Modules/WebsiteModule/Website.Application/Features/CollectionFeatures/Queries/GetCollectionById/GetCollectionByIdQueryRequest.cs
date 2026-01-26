using MediatR;

namespace Website.Application.Features.CollectionFeatures.Queries.GetCollectionById
{
    public class GetCollectionByIdQueryRequest : IRequest<GetCollectionByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
