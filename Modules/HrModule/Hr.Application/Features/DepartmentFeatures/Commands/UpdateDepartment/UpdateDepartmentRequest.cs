using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.UpdateDepartment
{
    public class UpdateDepartmentRequest : IRequest<UpdateDepartmentResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
