using Hr.Domain.Enums;
using MediatR;
using System;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Commands.UpdateSalaryStructureComponent
{
    public class UpdateSalaryStructureComponentRequest : IRequest<UpdateSalaryStructureComponentResponse>
    {
        public int Id { get; set; }
        public int SalaryStructureId { get; set; }
        public string Name { get; set; } = string.Empty;
        public PayrollComponentType Type { get; set; } 
        public decimal? FixedAmount { get; set; }
        public decimal? Percentage { get; set; }
    }
}