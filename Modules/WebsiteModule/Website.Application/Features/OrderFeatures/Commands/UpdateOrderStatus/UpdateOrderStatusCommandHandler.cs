using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.OrderFeatures.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommandRequest, UpdateOrderStatusCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateOrderStatusCommandResponse> Handle(UpdateOrderStatusCommandRequest request, CancellationToken cancellationToken)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var order = await orderRepo.GetByIdAsync(request.OrderId);

            if (order == null)
            {
                return new UpdateOrderStatusCommandResponse
                {
                    Success = false,
                    Message = "Order not found."
                };
            }

            // Update status
            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Publish OrderStatusChangedEvent if needed

            return new UpdateOrderStatusCommandResponse { Success = true };
        }
    }
}
