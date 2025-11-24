using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportParametersRepository
    {
        Task<Report.Domain.Entities.ReportParameter?> GetByIdAsync(int id);
        Task<IEnumerable<Report.Domain.Entities.ReportParameter>> GetAllAsync();
    }
}