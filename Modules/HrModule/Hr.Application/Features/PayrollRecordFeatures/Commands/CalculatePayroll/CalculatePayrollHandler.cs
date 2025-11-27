﻿using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.Features.PayrollRecordFeatures.Commands.CalculatePayroll;
using Hr.Domain.Entities;
using Hr.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.PayrollRecordFeatures.Commands.RecalculatePayroll
{
    public class CalculatePayrollHandler : IRequestHandler<CalculatePayrollRequest, CalculatePayrollResponse>
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CalculatePayrollHandler(
            IPayrollRecordRepository payrollRecordRepository,
            IEmployeeContractRepository employeeContractRepository,
            IUnitOfWork unitOfWork)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _employeeContractRepository = employeeContractRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CalculatePayrollResponse> Handle(CalculatePayrollRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var payrollRecord = await _payrollRecordRepository.GetByIdAsync(request.PayrollRecordId);
                if (payrollRecord == null)
                {
                    return new CalculatePayrollResponse
                    {
                        Success = false,
                        Message = "لم يتم العثور على سجل الرواتب"
                    };
                }

                var contract = await _employeeContractRepository.GetContractByEmployeeIdAsync(payrollRecord.EmployeeId);
                if (contract == null)
                {
                    return new CalculatePayrollResponse
                    {
                        Success = false,
                        Message = "لم يتم العثور على عقد نشط للموظف"
                    };
                }

                // Recalculate components based on salary structure
                decimal totalAllowances = 0;
                decimal totalDeductions = 0;

                foreach (var comp in payrollRecord?.Components ?? new List<PayrollComponent>())
                {
                    if
                        (
                            comp.ComponentType == PayrollComponentType.Allowance
                         || comp.ComponentType == PayrollComponentType.Overtime
                         || comp.ComponentType == PayrollComponentType.Bonus
                        )
                    {

                        var percentageAmount = (comp.Percentage ?? 0) * payrollRecord.BaseSalary / 100m;
                        totalAllowances += (comp.FixedAmount ?? 0) + percentageAmount;
                    }
                    else if
                        (
                             comp.ComponentType == PayrollComponentType.Deduction
                          || comp.ComponentType == PayrollComponentType.Insurance
                          || comp.ComponentType == PayrollComponentType.Tax
                          || comp.ComponentType == PayrollComponentType.Commission
                        )
                    {
                        var percentageAmount = (comp.Percentage ?? 0) * payrollRecord.BaseSalary / 100m;
                        totalDeductions += (comp.FixedAmount ?? 0) + percentageAmount;

                    }
                }

                // Update payroll totals
                payrollRecord.TotalAllowances = totalAllowances;
                payrollRecord.TotalDeductions = totalDeductions;
                payrollRecord.TotalGrossSalary = payrollRecord.BaseSalary + totalAllowances;
                payrollRecord.NetSalary = payrollRecord.TotalGrossSalary - totalDeductions;

                _payrollRecordRepository.Update(payrollRecord);
                await _unitOfWork.SaveChangesAsync();

                return new CalculatePayrollResponse
                {
                    Success = true,
                    Message = "تم إعادة حساب الرواتب بنجاح",
                    TotalAllowances = totalAllowances,
                    TotalDeductions = totalDeductions,
                    TotalGrossSalary = payrollRecord.TotalGrossSalary,
                    NetSalary = payrollRecord.NetSalary
                };
            }
            catch (Exception ex)
            {
                return new CalculatePayrollResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء إعادة حساب الرواتب"
                };
            }
        }
    }
}