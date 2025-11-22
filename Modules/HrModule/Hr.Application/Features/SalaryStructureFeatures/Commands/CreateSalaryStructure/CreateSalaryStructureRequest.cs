using Hr.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using Hr.Application.DTOs;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.CreateSalaryStructure
{
    public class CreateSalaryStructureRequest : IRequest<CreateSalaryStructureResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public SalaryStructureType Type { get; set; } 
        public bool IsActive { get; set; } = true;
        public ICollection<SalaryStructureComponentForCreationDto> Components { get; set; } = new List<SalaryStructureComponentForCreationDto>();
    }
}