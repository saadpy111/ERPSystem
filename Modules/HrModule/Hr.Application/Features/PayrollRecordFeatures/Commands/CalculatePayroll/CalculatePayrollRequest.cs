using Hr.Application.Features.PayrollRecordFeatures.Commands.RecalculatePayroll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Features.PayrollRecordFeatures.Commands.CalculatePayroll
{
    public class CalculatePayrollRequest : IRequest<CalculatePayrollResponse>
    {
        public int PayrollRecordId { get; set; }
    }
}