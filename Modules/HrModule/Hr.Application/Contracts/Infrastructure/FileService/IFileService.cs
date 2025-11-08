using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Infrastructure.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderPath);
        Task DeleteFileAsync(string filePath);
    }
}
