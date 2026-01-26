using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CartFeatures.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommandRequest, ClearCartCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClearCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClearCartCommandResponse> Handle(ClearCartCommandRequest request, CancellationToken cancellationToken)
        {
            var cartRepo = _unitOfWork.Repository<Cart>();
            var cartItemRepo = _unitOfWork.Repository<CartItem>();

            // Find the user's active cart (not checked out)
            var cart = await cartRepo.GetFirstAsync(
                c => c.UserId == request.UserId && !c.IsCheckedOut,
                asNoTracking: false,
                c => c.Items);

            if (cart != null && cart.Items.Any())
            {
                // Remove all items
                cartItemRepo.RemoveRange(cart.Items);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new ClearCartCommandResponse { Success = true };
        }
    }
}
