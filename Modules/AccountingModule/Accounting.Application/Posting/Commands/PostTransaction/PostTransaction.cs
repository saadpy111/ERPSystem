using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Domain.Enums;
using MediatR;

namespace Accounting.Application.Posting.Commands.PostTransaction
{
    public class PostTransactionCommand : IRequest<string>
    {
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
    }

    public class PostTransactionCommandHandler : IRequestHandler<PostTransactionCommand, string>
    {
        public Task<string> Handle(PostTransactionCommand request, CancellationToken cancellationToken)
        {
            // Placeholder: this normally delegates to the precise IPostingStrategy via factory
            return Task.FromResult("Posted Successfully");
        }
    }
}
