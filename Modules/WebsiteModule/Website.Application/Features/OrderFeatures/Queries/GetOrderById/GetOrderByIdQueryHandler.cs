using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OrderFeatures.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var order = await orderRepo.GetFirstAsync(
                o => o.Id == request.OrderId && o.UserId == request.UserId,
                asNoTracking: true,
                o => o.Items);

            if (order == null)
            {
                return new GetOrderByIdQueryResponse { Order = null };
            }

            var orderDto = new OrderDetailDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Street = order.ShippingAddress.Street,
                City = order.ShippingAddress.City,
                State = order.ShippingAddress.State,
                Country = order.ShippingAddress.Country,
                ZipCode = order.ShippingAddress.ZipCode,
                PaymentMethod = order.PaymentMethod,
                Notes = order.Notes,
                OrderDate = order.OrderDate,
                 DiscountTotal = order.DiscountTotal,
                  SubTotal = order.SubTotal,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductNameSnapshot,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    SubTotal = i.SubTotal,
                     AppliedOfferName = i.AppliedOfferName,
                      DiscountAmount = i.DiscountAmount,
                       FinalPrice = i.FinalPrice
                     
                }).ToList()
            };

            return new GetOrderByIdQueryResponse { Order = orderDto };
        }
    }
}

