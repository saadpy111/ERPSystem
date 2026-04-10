using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    public class VoucherPostingStrategy : IPostingStrategy
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherPostingStrategy(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Voucher;

        public async Task<List<JournalEntryLine>> GenerateLinesAsync(IPostingRequest request)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdWithLinesAsync(request.SourceId);
            if (voucher == null) 
                throw new Exception($"Voucher with ID {request.SourceId} not found.");

            return voucher.Lines.Select(line => new JournalEntryLine
            {
                AccountId = line.AccountId,
                Debit = line.Debit,
                Credit = line.Credit,
                Description = voucher.Description
            }).ToList();
        }
    }
}
