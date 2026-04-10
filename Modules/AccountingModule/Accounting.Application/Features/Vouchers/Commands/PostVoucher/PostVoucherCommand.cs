using Accounting.Application.Common.Models;
using MediatR;

namespace Accounting.Application.Features.Vouchers.Commands.PostVoucher
{
    public class PostVoucherCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public PostVoucherCommand(int id)
        {
            Id = id;
        }
    }
}
