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
            // 1. التحقق من المدخلات لتجنب إرسال قيم فارغة
            if (string.IsNullOrWhiteSpace(userPrompt)) userPrompt = "Select all data";

            var prompt = $@"
You are a SQL Server expert.
Given this database schema:
{schema}

Generate a valid SQL Server query for this request:
{userPrompt}

Return ONLY SQL without explanation, no markdown, no code blocks.
";

            // 2. بناء الجسم (Body)
            var body = new
            {
                // 👇 قم بتغيير السطر هذا
                model = "llama-3.3-70b-versatile", // هذا هو الموديل الأحدث والأقوى حالياً

                messages = new[]
                {
                       new { role = "user", content = prompt }
                },
                temperature = 0.1 // درجة حرارة منخفضة للدقة في الكود
            };

            // 3. إرسال الطلب
            var response = await _httpClient.PostAsJsonAsync(
                "chat/completions",
                body,
                cancellationToken
            );

            // 4. قراءة الرد (سواء نجح أو فشل)
            var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);

            // 5. التحقق من النجاح يدوياً لعرض رسالة الخطأ الحقيقية
            if (!response.IsSuccessStatusCode)
            {
                // هنا سيظهر لك سبب الـ 400 Bad Request
                throw new Exception($"AI API Error ({response.StatusCode}): {resultJson}");
            }

            // 6. استخراج النتيجة في حالة النجاح
            using var doc = JsonDocument.Parse(resultJson);

            // التحقق من وجود choices
            if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var sql = choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                // تنظيف الكود من أي Markdown قد يضيفه الذكاء الاصطناعي
                return CleanSql(sql);
            }

            return string.Empty;
        }

        private string CleanSql(string? sql)
        {
            if (string.IsNullOrEmpty(sql)) return string.Empty;

            // إزالة ```sql و ```
            sql = sql.Replace("```sql", "").Replace("```", "").Trim();
            return sql;
        }
    }
}