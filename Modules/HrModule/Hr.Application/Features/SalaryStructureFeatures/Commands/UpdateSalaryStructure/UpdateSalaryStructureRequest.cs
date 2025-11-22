using Hr.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using Hr.Application.DTOs;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.UpdateSalaryStructure
{
    public class UpdateSalaryStructureRequest : IRequest<UpdateSalaryStructureResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public SalaryStructureType Type { get; set; } 
        public bool IsActive { get; set; }
        public ICollection<SalaryStructureComponentForUpdateDto> Components { get; set; } = new List<SalaryStructureComponentForUpdateDto>();
    }
}