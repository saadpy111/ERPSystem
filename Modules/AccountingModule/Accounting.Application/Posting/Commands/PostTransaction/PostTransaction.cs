using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Domain.Enums;
using MediatR;
using Accounting.Application.Common.Models;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Interfaces.Repositories;

namespace Accounting.Application.Posting.Commands.PostTransaction
{
    public class PostTransactionCommand : IRequest<Result<int>>, IPostingRequest
    {
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Reference { get; set; }
        public int CurrencyId { get; set; }
    }

    public class PostTransactionCommandHandler : IRequestHandler<PostTransactionCommand, Result<int>>
    {
        private readonly IPostingService _postingService;
        private readonly IUnitOfWork _unitOfWork;

        public PostTransactionCommandHandler(IPostingService postingService , IUnitOfWork unitOfWork)
        {
            _postingService = postingService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(PostTransactionCommand request, CancellationToken cancellationToken)
        {
            var postResult = await _postingService.PostAsync(request);
            await _unitOfWork.SaveChangesAsync();

            if (postResult == null)
                return Result<int>.Failure("Unexpected null result from posting service.");

            if (!postResult.Success)
                return Result<int>.Failure(postResult.Message);

            return Result<int>.Ok(postResult.Data.Id, "Transaction posted successfully.");
        }
    }
}
