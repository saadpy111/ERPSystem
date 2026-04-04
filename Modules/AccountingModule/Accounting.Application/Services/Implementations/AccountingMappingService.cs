using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace Accounting.Application.Services.Implementations
{
    public class AccountingMappingService : IAccountingMappingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountingMappingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetAccountIdAsync(SourceType sourceType, string key)
        {
            var mapping = await _unitOfWork.AccountingMappings.GetByKeyAsync(sourceType, key);

            if (mapping == null || !mapping.IsActive)
            {
                throw new Exception($"BusinessException: Mapping not configured for {sourceType} - {key}");
            }

            return mapping.AccountId;
        }
    }
}
