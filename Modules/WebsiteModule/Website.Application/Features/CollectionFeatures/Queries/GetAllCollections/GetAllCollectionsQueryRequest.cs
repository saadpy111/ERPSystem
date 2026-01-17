using MediatR;

namespace Website.Application.Features.CollectionFeatures.Queries.GetAllCollections
{
    public class GetAllCollectionsQueryRequest : IRequest<GetAllCollectionsQueryResponse>
    {
        public bool? IsActive { get; set; }
    }
}
