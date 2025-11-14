using MediatR;

namespace Hr.Application.Features.SalaryStructureFeatures.Queries.GetAllSalaryStructures
{
    public class GetSalaryStructuresPagedRequest : IRequest<GetSalaryStructuresPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "Name";
        public bool IsDescending { get; set; } = false;
    }
}