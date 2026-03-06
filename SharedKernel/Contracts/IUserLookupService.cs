namespace SharedKernel.Contracts
{
    /// <summary>
    /// Read-only service for accessing Identity user data from other modules.
    /// Implemented by IdentityModule, consumed by WebsiteModule.
    /// Maintains module isolation — never reference Identity DbContext directly.
    /// </summary>
    public interface IUserLookupService
    {
        /// <summary>
        /// Get basic user info by user ID.
        /// Returns null if user not found.
        /// </summary>
        Task<UserLookupDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get multiple users by their IDs in one call.
        /// Returns only found entries.
        /// </summary>
        Task<List<UserLookupDto>> GetUsersByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search users by name or email and return their IDs.
        /// Used for cross-module filtering.
        /// </summary>
        Task<List<string>> SearchUserIdsByTermAsync(string term, CancellationToken cancellationToken = default);
    }

    public class UserLookupDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
