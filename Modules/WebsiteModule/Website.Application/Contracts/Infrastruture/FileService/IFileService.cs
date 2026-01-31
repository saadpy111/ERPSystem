using Microsoft.AspNetCore.Http;

namespace Website.Application.Contracts.Infrastruture.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderPath);
        Task<string> CopyFileAsync(string sourceRelativePath, string destinationFolderPath);
        Task DeleteFileAsync(string filePath);
    }
}
