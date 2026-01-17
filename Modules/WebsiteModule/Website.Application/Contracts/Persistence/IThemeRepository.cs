using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence
{
    /// <summary>
    /// Repository interface for Theme entity.
    /// </summary>
    public interface IThemeRepository
    {
        Task<Theme?> GetByIdAsync(Guid id);
        Task<Theme?> GetByCodeAsync(string code);
        Task<List<Theme>> GetAllAsync(bool activeOnly = true);
        Task<Theme> CreateAsync(Theme theme);
        Task<Theme> UpdateAsync(Theme theme);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(string code);
    }
}
