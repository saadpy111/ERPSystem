using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, DeleteBrandCommandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public DeleteBrandCommandHandler(
            IBrandRepository brandRepository,
            IWebsiteUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteBrandCommandResponse> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return new DeleteBrandCommandResponse { Success = false, Error = "Invalid Id" };

            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
                return new DeleteBrandCommandResponse { Success = false, Error = "Brand not found" };

            await _brandRepository.DeleteAsync(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteBrandCommandResponse { Success = true };
        }
    }
}
