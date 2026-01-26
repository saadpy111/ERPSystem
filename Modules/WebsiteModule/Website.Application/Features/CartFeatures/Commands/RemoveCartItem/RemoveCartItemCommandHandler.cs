using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CartFeatures.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommandRequest, RemoveCartItemCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCartItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RemoveCartItemCommandResponse> Handle(RemoveCartItemCommandRequest request, CancellationToken cancellationToken)
        {
            var cartItemRepo = _unitOfWork.Repository<CartItem>();
            var item = await cartItemRepo.GetByIdAsync(request.ItemId);

            if (item == null)
            {
                return new RemoveCartItemCommandResponse
                {
                    Success = false,
                    Message = "Cart item not found."
                };
            }

            cartItemRepo.Remove(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RemoveCartItemCommandResponse { Success = true };
        }
    }
}
