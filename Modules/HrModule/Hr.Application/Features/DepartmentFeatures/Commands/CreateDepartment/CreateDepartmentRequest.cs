using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.CreateDepartment
{
    public class CreateDepartmentRequest : IRequest<CreateDepartmentResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
