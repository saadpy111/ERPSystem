using Microsoft.AspNetCore.Http;
using SharedKernel.Core.Files;

namespace Inventory.Api.Services
{
    public class FileUrlResolver : IFileUrlResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? Resolve(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            if (relativePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return relativePath;

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                return relativePath;

            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return $"{baseUrl}/{relativePath.TrimStart('/')}";
        }
    }
}
