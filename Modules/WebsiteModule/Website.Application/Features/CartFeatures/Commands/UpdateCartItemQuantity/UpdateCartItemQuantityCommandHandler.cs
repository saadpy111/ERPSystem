using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CartFeatures.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommandRequest, UpdateCartItemQuantityCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateCartItemQuantityCommandResponse> Handle(UpdateCartItemQuantityCommandRequest request, CancellationToken cancellationToken)
        {
            var cartItemRepo = _unitOfWork.Repository<CartItem>();
            var item = await cartItemRepo.GetByIdAsync(request.ItemId);

            if (item == null)
            {
                return new UpdateCartItemQuantityCommandResponse
                {
                    Success = false,
                    Message = "Cart item not found."
                };
            }

            // If quantity is 0 or less, remove the item
            if (request.Quantity <= 0)
            {
                cartItemRepo.Remove(item);
            }
            else
            {
                // Update quantity
                item.Quantity = request.Quantity;
                item.UpdatedAt = DateTime.UtcNow;
                cartItemRepo.Update(item);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateCartItemQuantityCommandResponse { Success = true };
        }
    }
}
