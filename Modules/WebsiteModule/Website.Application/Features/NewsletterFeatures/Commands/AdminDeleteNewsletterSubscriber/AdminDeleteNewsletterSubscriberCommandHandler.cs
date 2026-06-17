using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.NewsletterFeatures.Commands.AdminDeleteNewsletterSubscriber
{
    public class AdminDeleteNewsletterSubscriberCommandHandler : IRequestHandler<AdminDeleteNewsletterSubscriberCommand, bool>
    {
        private readonly INewsletterSubscriberRepository _repository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public AdminDeleteNewsletterSubscriberCommandHandler(INewsletterSubscriberRepository repository, IWebsiteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AdminDeleteNewsletterSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = await _repository.GetByIdAsync(request.Id);
            if (subscriber == null)
                return false;

            subscriber.IsActive = false;
            subscriber.UpdatedAt = System.DateTime.UtcNow;

            _repository.Update(subscriber);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
