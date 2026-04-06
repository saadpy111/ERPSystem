using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Domain.Enums;
using MediatR;
using Accounting.Application.Posting.Interfaces;

namespace Accounting.Application.Posting.Commands.PostTransaction
{
    public class PostTransactionCommand : IRequest<string>, IPostingRequest
    {
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Reference { get; set; }
        public int CurrencyId { get; set; }
    }

    public class PostTransactionCommandHandler : IRequestHandler<PostTransactionCommand, string>
    {
        private readonly IPostingService _postingService;

        public PostTransactionCommandHandler(IPostingService postingService)
        {
            _postingService = postingService;
        }

        public async Task<string> Handle(PostTransactionCommand request, CancellationToken cancellationToken)
        {
            var resultId = await _postingService.PostAsync(request);
            return resultId.ToString();
        }
    }
}
