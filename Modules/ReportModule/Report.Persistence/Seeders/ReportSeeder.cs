using Report.Domain.Entities;
using Report.Domain.Enums;
using Report.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.Persistence.Seeders
{
    public class ReportSeeder
    {
        private readonly ReportDbContext _context;

        public ReportSeeder(ReportDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedDataSourcesAsync();
            await SeedEmployeesReportAsync();
        }

        private async Task SeedDataSourcesAsync()
        {
            if (!_context.ReportDataSources.Any())
            {
                var ds = new ReportDataSource
                {
                    Name = "HR_Main",
                    Type = DataSourceType.SqlServer,
                    ConnectionString = "Server=.;Database=HR;Trusted_Connection=True;"
                };

                _context.ReportDataSources.Add(ds);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedEmployeesReportAsync()
        {
            if (_context.Reports.Any(r => r.Name == "Employees Master Report"))
                return;

            var report = new Report.Domain.Entities.Report
            {
                Name = "Employees Master Report",
                Description = "Full employee list with all HR details",
                IsActive = true,
                Query = "" 
            };

            // ---------------------- FIELDS ----------------------
            var fields = new List<ReportField>
            {
                // Employee
                new ReportField { Name = "EmployeeId", DisplayName = "Employee ID", Expression = "E.EmployeeId", Type = FieldType.Number },
                new ReportField { Name = "FullName", DisplayName = "Full Name", Expression = "E.FullName", Type = FieldType.Text },
                new ReportField { Name = "Email", DisplayName = "Email", Expression = "E.Email", Type = FieldType.Text },
                new ReportField { Name = "PhoneNumber", DisplayName = "Phone", Expression = "E.PhoneNumber", Type = FieldType.Text },
                new ReportField { Name = "Gender", DisplayName = "Gender", Expression = "E.Gender", Type = FieldType.Text },
                new ReportField { Name = "DateOfBirth", DisplayName = "Birthdate", Expression = "E.DateOfBirth", Type = FieldType.Date },
                new ReportField { Name = "Address", DisplayName = "Address", Expression = "E.Address", Type = FieldType.Text },
                new ReportField { Name = "EmployeeStatus", DisplayName = "Status", Expression = "E.Status", Type = FieldType.Text },

                // Manager
                new ReportField { Name = "ManagerName", DisplayName = "Manager", Expression = "M.FullName", Type = FieldType.Text },

                // Department
                new ReportField { Name = "Department", DisplayName = "Department", Expression = "D.Name", Type = FieldType.Text },

                // Job
                new ReportField { Name = "JobTitle", DisplayName = "Job Title", Expression = "J.Title", Type = FieldType.Text },
                new ReportField { Name = "WorkType", DisplayName = "Work Type", Expression = "J.WorkType", Type = FieldType.Text },
                new ReportField { Name = "JobStatus", DisplayName = "Job Status", Expression = "J.Status", Type = FieldType.Text },
                new ReportField { Name = "Responsibilities", DisplayName = "Responsibilities", Expression = "J.Responsibilities", Type = FieldType.Text },
                new ReportField { Name = "RequiredSkills", DisplayName = "Required Skills", Expression = "J.RequiredSkills", Type = FieldType.Text },
                new ReportField { Name = "RequiredExperience", DisplayName = "Required Experience", Expression = "J.RequiredExperience", Type = FieldType.Text },
                new ReportField { Name = "RequiredQualification", DisplayName = "Required Qualification", Expression = "J.RequiredQualification", Type = FieldType.Text },

                // Contract
                new ReportField { Name = "Salary", DisplayName = "Salary", Expression = "C.Salary", Type = FieldType.Currency },
                new ReportField { Name = "ContractType", DisplayName = "Contract Type", Expression = "C.ContractType", Type = FieldType.Text },
                new ReportField { Name = "ContractStartDate", DisplayName = "Start Date", Expression = "C.StartDate", Type = FieldType.Date },
                new ReportField { Name = "ContractEndDate", DisplayName = "End Date", Expression = "C.EndDate", Type = FieldType.Date },
                new ReportField { Name = "ProbationPeriod", DisplayName = "Probation Period", Expression = "C.ProbationPeriod", Type = FieldType.Number }
            };

            foreach (var f in fields)
                report.Fields.Add(f);


            // ---------------------- PARAMETERS ----------------------
            var parameters = new List<ReportParameter>
            {
                new ReportParameter { Name = "DepartmentId", DisplayName = "Department", DataType = ParameterDataType.Integer },
                new ReportParameter { Name = "JobId", DisplayName = "Job", DataType = ParameterDataType.Integer },
                new ReportParameter { Name = "Status", DisplayName = "Status", DataType = ParameterDataType.String },
                new ReportParameter { Name = "Gender", DisplayName = "Gender", DataType = ParameterDataType.String },
                new ReportParameter { Name = "HireDateFrom", DisplayName = "Hire Date From", DataType = ParameterDataType.DateTime },
                new ReportParameter { Name = "HireDateTo", DisplayName = "Hire Date To", DataType = ParameterDataType.DateTime },
                new ReportParameter { Name = "ContractType", DisplayName = "Contract Type", DataType = ParameterDataType.String },
                new ReportParameter { Name = "MinSalary", DisplayName = "Min Salary", DataType = ParameterDataType.Decimal },
                new ReportParameter { Name = "MaxSalary", DisplayName = "Max Salary", DataType = ParameterDataType.Decimal }
            };

            foreach (var p in parameters)
                report.Parameters.Add(p);


            // ---------------------- FILTERS ----------------------
            var filters = new List<ReportFilter>
            {
                new ReportFilter { FieldName = "E.DepartmentId", Operator = FilterOperator.Equal, ParameterName = "DepartmentId" },
                new ReportFilter { FieldName = "E.JobId", Operator = FilterOperator.Equal, ParameterName = "JobId" },
                new ReportFilter { FieldName = "E.Status", Operator = FilterOperator.Equal, ParameterName = "Status" },
                new ReportFilter { FieldName = "E.Gender", Operator = FilterOperator.Equal, ParameterName = "Gender" },
                new ReportFilter { FieldName = "C.ContractType", Operator = FilterOperator.Equal, ParameterName = "ContractType" },
                new ReportFilter { FieldName = "C.Salary", Operator = FilterOperator.GreaterThanOrEqual, ParameterName = "MinSalary" },
                new ReportFilter { FieldName = "C.Salary", Operator = FilterOperator.LessThanOrEqual, ParameterName = "MaxSalary" },
                new ReportFilter { FieldName = "E.HireDate", Operator = FilterOperator.GreaterThanOrEqual, ParameterName = "HireDateFrom" },
                new ReportFilter { FieldName = "E.HireDate", Operator = FilterOperator.LessThanOrEqual, ParameterName = "HireDateTo" }
            };

            foreach (var f in filters)
                report.Filters.Add(f);


            // ---------------------- SORTING ----------------------
            report.Sortings.Add(new ReportSorting
            {
                Expression = "E.FullName",
                SortOrder = 1,
                Direction = SortDirection.Ascending
            });

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }
    }
}
