using Hr.Application.Contracts.Infrastructure.FileService;
using Microsoft.AspNetCore.Http;

namespace Hr.Infrastructure.FileService
{
    public class HrLocalFileService : IFileService
    {
        private readonly string _rootPath;

        public HrLocalFileService(string rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.", nameof(file));

            var fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            string targetDirectory = Path.Combine(_rootPath, folderPath);

            Directory.CreateDirectory(targetDirectory);

            string fullPath = Path.Combine(targetDirectory, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderPath, uniqueFileName);
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            var fullPath = Path.Combine(_rootPath, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Task.CompletedTask;
        }
    }
}
