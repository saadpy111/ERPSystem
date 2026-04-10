using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class CostCenterRepository : GenericRepository<CostCenter>, ICostCenterRepository
    {
        public CostCenterRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            return !await _dbSet.AnyAsync(cc => cc.Code == code && (!excludeId.HasValue || cc.Id != excludeId));
        }

        public async Task<bool> HasTransactionsAsync(int id)
        {
            var usedInJournalLines = await _context.JournalEntryLines.AnyAsync(l => l.CostCenterId == id);
            var usedInVoucherLines = await _context.VoucherLines.AnyAsync(l => l.CostCenterId == id);
            
            return usedInJournalLines || usedInVoucherLines;
        }

        public async Task<List<CostCenter>> GetHierarchyAsync()
        {
            return await _dbSet
                .Include(cc => cc.Children)
                .Where(cc => cc.ParentId == null)
                .ToListAsync();
        }

        public async Task<bool> IsDescendantAsync(int parentId, int childId)
        {
            var currentParentId = await _dbSet
                .Where(cc => cc.Id == childId)
                .Select(cc => cc.ParentId)
                .FirstOrDefaultAsync();

            while (currentParentId != null)
            {
                if (currentParentId == parentId)
                    return true;

                currentParentId = await _dbSet
                    .Where(cc => cc.Id == currentParentId)
                    .Select(cc => cc.ParentId)
                    .FirstOrDefaultAsync();
            }

            return false;
        }
    }
}
