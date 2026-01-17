namespace Identity.Application.Contracts.Persistence
{
    public interface IPermissionRepository
    {
        Task<List<string>> GetUserEffectivePermissionsAsync(string userId);
    }
}
