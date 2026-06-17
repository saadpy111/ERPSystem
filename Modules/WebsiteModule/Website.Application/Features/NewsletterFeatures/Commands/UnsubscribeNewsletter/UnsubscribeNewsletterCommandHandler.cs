using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;

namespace Website.Application.Features.NewsletterFeatures.Commands.UnsubscribeNewsletter
{
    public class UnsubscribeNewsletterCommandHandler : IRequestHandler<UnsubscribeNewsletterCommand, bool>
    {
        private readonly INewsletterSubscriberRepository _repository;
        private readonly IWebsiteUnitOfWork _unitOfWork;

        public UnsubscribeNewsletterCommandHandler(INewsletterSubscriberRepository repository, IWebsiteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UnsubscribeNewsletterCommand request, CancellationToken cancellationToken)
        {
            var subscriber = await _repository.GetByIdAsync(request.Id);
            if (subscriber == null)
                return false;

            subscriber.IsActive = false;
            subscriber.UnsubscribedAt = System.DateTime.UtcNow;
            subscriber.UpdatedAt = System.DateTime.UtcNow;

            _repository.Update(subscriber);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
