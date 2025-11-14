using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.CreateEmployeeContract
{
    public class CreateEmployeeContractRequest : IRequest<CreateEmployeeContractResponse>
    {
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public int? SalaryStructureId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public ContractType ContractType { get; set; }
        public int? ProbationPeriod { get; set; }
        public string? Notes { get; set; }
        public ICollection<IFormFile>? AttachmentFiles { get; set; }
    }
}