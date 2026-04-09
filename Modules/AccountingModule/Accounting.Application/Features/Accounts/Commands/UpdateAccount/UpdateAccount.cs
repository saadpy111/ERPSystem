using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Common.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
    }

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Result>
    {
        private readonly IAccountingDbContext _context;

        public UpdateAccountCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            if (account == null) return Result.Failure("Account not found");

            account.NameAr = request.NameAr;
            account.NameEn = request.NameEn;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.IsSuccess("Account updated successfully");
        }
    }
}
