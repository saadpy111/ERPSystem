namespace Subscription.Application.Features.Modules.Queries.GetEffectiveModules
{
    public class GetEffectiveModulesResponse
    {
        public bool Success { get; set; }
        public List<EffectiveModuleDto> Data { get; set; } = new();
    }

    public class EffectiveModuleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}
