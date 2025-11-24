using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ReportDbContext _context;

        public UnitOfWork(ReportDbContext context)
        {
             _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
