using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Report.Application.Contracts.Persistence.Repositories;
using SqlKata;
using SqlKata.Compilers;
using System.Data;

namespace Report.Persistence.QueryEngine
{
    public class ReportQueryEngine : IReportQueryEngine
    {
        private readonly Compiler _compiler;
        private readonly IConfiguration _configuration;

        public ReportQueryEngine(IConfiguration configuration)
        {
            _configuration = configuration;
            _compiler = new SqlServerCompiler();

        }

        public async Task<IEnumerable<IDictionary<string, object?>>> ExecuteAsync
                (Query query)
        {
           
            var compiled = _compiler.Compile(query);

            using var conn = new SqlConnection(
                                       _configuration.GetConnectionString("ConnectionString"));
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var rows = await conn.QueryAsync(compiled.Sql, compiled.NamedBindings);
            return rows.Select(r => (IDictionary<string, object?>)r);
        }


        public async Task<IEnumerable<IDictionary<string, object?>>> ExecuteAsync(string sql)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("ConnectionString"));

            var result = await connection.QueryAsync(sql);
            return result.Select(r => (IDictionary<string, object?>)r);
        }

        public async Task<long> CountAsync(Query query)
        {
            var compiled = _compiler.Compile(query);
            var countSql = $"SELECT COUNT(1) AS Total FROM ({compiled.Sql}) AS t";

            using var conn = new SqlConnection(
                                       _configuration.GetConnectionString("ConnectionString"));
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var total = await conn.ExecuteScalarAsync<long>(countSql, compiled.NamedBindings);
            return total;
        }
    }
}
