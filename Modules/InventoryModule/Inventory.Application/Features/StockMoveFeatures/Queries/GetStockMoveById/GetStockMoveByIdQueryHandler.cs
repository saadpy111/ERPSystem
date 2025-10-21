//using Inventory.Application.Contracts.Persistence.Repositories;
//using Inventory.Application.Dtos.StockMoveDtos;
//using Inventory.Domain.Entities;
//using MediatR;

//namespace Inventory.Application.Features.StockMoveFeatures.Queries.GetStockMoveById
//{
//    public class GetStockMoveByIdQueryHandler : IRequestHandler<GetStockMoveByIdQueryRequest, GetStockMoveByIdQueryResponse>
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public GetStockMoveByIdQueryHandler(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<GetStockMoveByIdQueryResponse> Handle(GetStockMoveByIdQueryRequest request, CancellationToken cancellationToken)
//        {
//            var entity = await _unitOfWork.Repositories<StockMove>().GetById(request.Id);
//            if (entity == null)
//                return new GetStockMoveByIdQueryResponse();

//            return new GetStockMoveByIdQueryResponse
//            {
//                StockMove = new GetStockMoveDto
//                {
//                    Id = entity.Id,
//                    Quantity = entity.Quantity,
//                    MoveDate = entity.MoveDate,
//                    Reference = entity.Reference,
//                    MoveType = entity.MoveType,
//                    ProductId = entity.ProductId,
//                    SourceLocationId = entity.SourceLocationId,
//                    DestinationLocationId = entity.DestinationLocationId,
//                    CreatedAt = entity.CreatedAt,
//                    UpdatedAt = entity.UpdatedAt
//                }
//            };
//        }
//    }
//}