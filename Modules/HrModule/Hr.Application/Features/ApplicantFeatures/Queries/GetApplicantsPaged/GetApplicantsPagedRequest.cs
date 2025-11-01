using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsPaged
{
    public class GetApplicantsPagedRequest : IRequest<GetApplicantsPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "FullName";
        public bool IsDescending { get; set; } = false;
        public int? JobId { get; set; }
        public int? CurrentStageId { get; set; }
        public string? Status { get; set; }
    }
}
