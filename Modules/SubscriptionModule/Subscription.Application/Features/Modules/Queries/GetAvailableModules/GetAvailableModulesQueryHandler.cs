using MediatR;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.DTOs;

namespace Subscription.Application.Features.Modules.Queries.GetAvailableModules
{
    public class GetAvailableModulesQueryHandler : IRequestHandler<GetAvailableModulesQuery, GetAvailableModulesResponse>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModulePriceRepository _modulePriceRepository;

        public GetAvailableModulesQueryHandler(
            IModuleRepository moduleRepository,
            IModulePriceRepository modulePriceRepository)
        {
            _moduleRepository = moduleRepository;
            _modulePriceRepository = modulePriceRepository;
        }

        public async Task<GetAvailableModulesResponse> Handle(GetAvailableModulesQuery request, CancellationToken cancellationToken)
        {
            var modules = await _moduleRepository.GetAllActiveAsync();

            var result = new List<ModuleDto>();

            foreach (var m in modules)
            {
                var prices = await _modulePriceRepository.GetByModuleIdAsync(m.Id);
                var filteredPrices = string.IsNullOrEmpty(request.CurrencyCode)
                    ? prices
                    : prices.Where(p => p.CurrencyCode.Equals(request.CurrencyCode, StringComparison.OrdinalIgnoreCase)).ToList();

                result.Add(new ModuleDto
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    DisplayName = m.DisplayName,
                    Description = m.Description,
                    Prices = filteredPrices.Select(p => new ModulePriceDto
                    {
                        CurrencyCode = p.CurrencyCode,
                        UnitPrice = p.UnitPrice,
                        Interval = p.Interval.ToString(),
                        EffectiveFrom = p.EffectiveFrom,
                        EffectiveTo = p.EffectiveTo
                    }).ToList()
                });
            }

            return new GetAvailableModulesResponse
            {
                Success = true,
                Data = result
            };
        }
    }
}
