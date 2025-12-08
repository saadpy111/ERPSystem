using System.Net.Http.Json;
using System.Text.Json;

namespace Report.Application.Services
{
    public class AIQueryBuilderService
    {
        private readonly HttpClient _httpClient;

        public AIQueryBuilderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateSqlAsync(
            string schema,
            string userPrompt,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userPrompt)) userPrompt = "Select all data";

            var prompt = $@"
                 You are a SQL Server expert.
                 Given this database schema:
                 {schema}
                 
                 Generate a valid SQL Server query for this request:
                 {userPrompt}
                 
                 Return ONLY SQL without explanation, no markdown, no code blocks.
                 ";

            var body = new
            {
                model = "llama-3.1-8b-instant",

                messages = new[]
                {
                       new { role = "user", content = prompt }
                },
                temperature = 0.1
            };

            var response = await _httpClient.PostAsJsonAsync(
                "chat/completions",
                body,
                cancellationToken
            );

            var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"AI API Error ({response.StatusCode}): {resultJson}");
            }

            using var doc = JsonDocument.Parse(resultJson);

            if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var sql = choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return CleanSql(sql);
            }

            return string.Empty;
        }

        private string CleanSql(string? sql)
        {
            if (string.IsNullOrEmpty(sql)) return string.Empty;

            sql = sql.Replace("```sql", "").Replace("```", "").Trim();
            return sql;
        }
    }
}