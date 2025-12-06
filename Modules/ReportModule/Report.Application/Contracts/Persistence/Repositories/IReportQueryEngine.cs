using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlKata;
namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IReportQueryEngine
    {
        Task<IEnumerable<IDictionary<string, object?>>> ExecuteAsync(Query query);
        Task<long> CountAsync(Query query);
        Task<IEnumerable<IDictionary<string, object?>>> ExecuteAsync(string sql);

    }

}
