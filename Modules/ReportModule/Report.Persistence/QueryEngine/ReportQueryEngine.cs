using Microsoft.Data.SqlClient;
using Report.Application.Contracts.Persistence.Repositories;
using SqlKata.Compilers;

using System.Data;
using Dapper;
using SqlKata;

namespace Report.Persistence.QueryEngine
{
    public class ReportQueryEngine : IReportQueryEngine
    {
        private readonly Compiler _compiler;

        public ReportQueryEngine()
        {
            _compiler = new SqlServerCompiler();
        }

        public async Task<IEnumerable<IDictionary<string, object?>>> ExecuteAsync
                (Query query, string connectionString)
        {
            var compiled = _compiler.Compile(query);

            using var conn = new SqlConnection(connectionString);
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var rows = await conn.QueryAsync(compiled.Sql, compiled.NamedBindings);
            return rows.Select(r => (IDictionary<string, object?>)r);
        }

        public async Task<long> CountAsync(Query query, string connectionString)
        {
            // compile the query and wrap it for counting
            var compiled = _compiler.Compile(query);
            var countSql = $"SELECT COUNT(1) AS Total FROM ({compiled.Sql}) AS t";

            using var conn = new SqlConnection(connectionString);
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var total = await conn.ExecuteScalarAsync<long>(countSql, compiled.NamedBindings);
            return total;
        }
    }
}
