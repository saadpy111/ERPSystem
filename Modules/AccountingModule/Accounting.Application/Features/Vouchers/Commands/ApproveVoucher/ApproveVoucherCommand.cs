using Accounting.Application.Common.Models;
using MediatR;

namespace Accounting.Application.Features.Vouchers.Commands.ApproveVoucher
{
    public class ApproveVoucherCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public ApproveVoucherCommand(int id)
        {
            Id = id;
        }
    }
}
