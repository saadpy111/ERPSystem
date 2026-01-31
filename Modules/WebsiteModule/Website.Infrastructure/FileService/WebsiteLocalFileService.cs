using Microsoft.AspNetCore.Http;
using Website.Application.Contracts.Infrastruture.FileService;
using Microsoft.AspNetCore.Hosting;

namespace Website.Infrastructure.FileService
{
    public class WebsiteLocalFileService : IFileService
    {
        private readonly string _rootPath;

        public WebsiteLocalFileService(IWebHostEnvironment env)
        {
            _rootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
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

            return Path.Combine(folderPath, uniqueFileName).Replace("\\", "/");
        }

        public async Task<string> CopyFileAsync(string sourceRelativePath, string destinationFolderPath)
        {
            if (string.IsNullOrEmpty(sourceRelativePath))
                throw new ArgumentException("Source relative path cannot be null or empty.", nameof(sourceRelativePath));

            string sourceFullPath = Path.Combine(_rootPath, sourceRelativePath);
            if (!File.Exists(sourceFullPath))
            {
                throw new FileNotFoundException("Source file not found.", sourceFullPath);
            }

            var fileExtension = Path.GetExtension(sourceRelativePath);
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            string targetDirectory = Path.Combine(_rootPath, destinationFolderPath);
            Directory.CreateDirectory(targetDirectory);

            string destinationFullPath = Path.Combine(targetDirectory, uniqueFileName);

            await Task.Run(() => File.Copy(sourceFullPath, destinationFullPath));

            return Path.Combine(destinationFolderPath, uniqueFileName).Replace("\\", "/");
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.CompletedTask;

            var fullPath = Path.Combine(_rootPath, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Task.CompletedTask;
        }
    }
}
