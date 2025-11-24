using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportFieldsRepository
    {
        Task<Report.Domain.Entities.ReportField?> GetByIdAsync(int id);
        Task<IEnumerable<Report.Domain.Entities.ReportField>> GetAllAsync();
    }
}