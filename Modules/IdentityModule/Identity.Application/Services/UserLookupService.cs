using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Contracts;

namespace Identity.Application.Services
{
    /// <summary>
    /// Implementation of IUserLookupService for cross-module user data access.
    /// Provides read-only access to basic user profile information.
    /// </summary>
    public class UserLookupService : IUserLookupService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserLookupService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserLookupDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new UserLookupDto
                {
                    UserId = u.Id,
                    FullName = u.FullName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    PhoneNumber = u.PhoneNumber
                })
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        public async Task<List<UserLookupDto>> GetUsersByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
        {
            if (userIds == null || !userIds.Any())
                return new List<UserLookupDto>();

            var users = await _userManager.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new UserLookupDto
                {
                    UserId = u.Id,
                    FullName = u.FullName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    PhoneNumber = u.PhoneNumber
                })
                .ToListAsync(cancellationToken);

            return users;
        }

        public async Task<List<string>> SearchUserIdsByTermAsync(string term, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<string>();

            var normalizedTerm = term.ToLower();

            return await _userManager.Users
                .AsNoTracking()
                .Where(u => (u.FullName != null && u.FullName.ToLower().Contains(normalizedTerm)) || 
                            (u.Email != null && u.Email.ToLower().Contains(normalizedTerm)))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
