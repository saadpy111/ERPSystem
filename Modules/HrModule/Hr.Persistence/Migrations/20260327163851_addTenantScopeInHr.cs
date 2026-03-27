using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTenantScopeInHr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "SalaryStructures",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "SalaryStructureComponents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "RecruitmentStages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "PayrollRecords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "PayrollComponents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "LoanInstallments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "LeaveTypes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "LeaveRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "EmployeeContracts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "AttendanceRecords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "Attachments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "Applicants",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "ApplicantExperiences",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Hr",
                table: "ApplicantEducations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_TenantId",
                schema: "Hr",
                table: "SalaryStructures",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_TenantId_Id",
                schema: "Hr",
                table: "SalaryStructures",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureComponents_TenantId",
                schema: "Hr",
                table: "SalaryStructureComponents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructureComponents_TenantId_Id",
                schema: "Hr",
                table: "SalaryStructureComponents",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentStages_TenantId",
                schema: "Hr",
                table: "RecruitmentStages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentStages_TenantId_StageId",
                schema: "Hr",
                table: "RecruitmentStages",
                columns: new[] { "TenantId", "StageId" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecords_TenantId",
                schema: "Hr",
                table: "PayrollRecords",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecords_TenantId_PayrollId",
                schema: "Hr",
                table: "PayrollRecords",
                columns: new[] { "TenantId", "PayrollId" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollComponents_TenantId",
                schema: "Hr",
                table: "PayrollComponents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollComponents_TenantId_ComponentId",
                schema: "Hr",
                table: "PayrollComponents",
                columns: new[] { "TenantId", "ComponentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_TenantId",
                schema: "Hr",
                table: "Loans",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_TenantId_LoanId",
                schema: "Hr",
                table: "Loans",
                columns: new[] { "TenantId", "LoanId" });

            migrationBuilder.CreateIndex(
                name: "IX_LoanInstallments_TenantId",
                schema: "Hr",
                table: "LoanInstallments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanInstallments_TenantId_InstallmentId",
                schema: "Hr",
                table: "LoanInstallments",
                columns: new[] { "TenantId", "InstallmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_TenantId",
                schema: "Hr",
                table: "LeaveTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_TenantId_LeaveTypeId",
                schema: "Hr",
                table: "LeaveTypes",
                columns: new[] { "TenantId", "LeaveTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_TenantId",
                schema: "Hr",
                table: "LeaveRequests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_TenantId_RequestId",
                schema: "Hr",
                table: "LeaveRequests",
                columns: new[] { "TenantId", "RequestId" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TenantId",
                schema: "Hr",
                table: "Jobs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TenantId_JobId",
                schema: "Hr",
                table: "Jobs",
                columns: new[] { "TenantId", "JobId" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TenantId",
                schema: "Hr",
                table: "Employees",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TenantId_EmployeeId",
                schema: "Hr",
                table: "Employees",
                columns: new[] { "TenantId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContracts_TenantId",
                schema: "Hr",
                table: "EmployeeContracts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContracts_TenantId_Id",
                schema: "Hr",
                table: "EmployeeContracts",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TenantId",
                schema: "Hr",
                table: "Departments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TenantId_DepartmentId",
                schema: "Hr",
                table: "Departments",
                columns: new[] { "TenantId", "DepartmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_TenantId",
                schema: "Hr",
                table: "AttendanceRecords",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_TenantId_RecordId",
                schema: "Hr",
                table: "AttendanceRecords",
                columns: new[] { "TenantId", "RecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TenantId",
                schema: "Hr",
                table: "Attachments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TenantId_Id",
                schema: "Hr",
                table: "Attachments",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_TenantId",
                schema: "Hr",
                table: "Applicants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_TenantId_ApplicantId",
                schema: "Hr",
                table: "Applicants",
                columns: new[] { "TenantId", "ApplicantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantExperiences_TenantId",
                schema: "Hr",
                table: "ApplicantExperiences",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantExperiences_TenantId_Id",
                schema: "Hr",
                table: "ApplicantExperiences",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantEducations_TenantId",
                schema: "Hr",
                table: "ApplicantEducations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantEducations_TenantId_Id",
                schema: "Hr",
                table: "ApplicantEducations",
                columns: new[] { "TenantId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalaryStructures_TenantId",
                schema: "Hr",
                table: "SalaryStructures");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructures_TenantId_Id",
                schema: "Hr",
                table: "SalaryStructures");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructureComponents_TenantId",
                schema: "Hr",
                table: "SalaryStructureComponents");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructureComponents_TenantId_Id",
                schema: "Hr",
                table: "SalaryStructureComponents");

            migrationBuilder.DropIndex(
                name: "IX_RecruitmentStages_TenantId",
                schema: "Hr",
                table: "RecruitmentStages");

            migrationBuilder.DropIndex(
                name: "IX_RecruitmentStages_TenantId_StageId",
                schema: "Hr",
                table: "RecruitmentStages");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRecords_TenantId",
                schema: "Hr",
                table: "PayrollRecords");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRecords_TenantId_PayrollId",
                schema: "Hr",
                table: "PayrollRecords");

            migrationBuilder.DropIndex(
                name: "IX_PayrollComponents_TenantId",
                schema: "Hr",
                table: "PayrollComponents");

            migrationBuilder.DropIndex(
                name: "IX_PayrollComponents_TenantId_ComponentId",
                schema: "Hr",
                table: "PayrollComponents");

            migrationBuilder.DropIndex(
                name: "IX_Loans_TenantId",
                schema: "Hr",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_TenantId_LoanId",
                schema: "Hr",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_LoanInstallments_TenantId",
                schema: "Hr",
                table: "LoanInstallments");

            migrationBuilder.DropIndex(
                name: "IX_LoanInstallments_TenantId_InstallmentId",
                schema: "Hr",
                table: "LoanInstallments");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_TenantId",
                schema: "Hr",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_TenantId_LeaveTypeId",
                schema: "Hr",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_TenantId",
                schema: "Hr",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_TenantId_RequestId",
                schema: "Hr",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TenantId",
                schema: "Hr",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TenantId_JobId",
                schema: "Hr",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TenantId",
                schema: "Hr",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TenantId_EmployeeId",
                schema: "Hr",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContracts_TenantId",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContracts_TenantId_Id",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.DropIndex(
                name: "IX_Departments_TenantId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_TenantId_DepartmentId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_TenantId",
                schema: "Hr",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceRecords_TenantId_RecordId",
                schema: "Hr",
                table: "AttendanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_TenantId",
                schema: "Hr",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_TenantId_Id",
                schema: "Hr",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_TenantId",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_TenantId_ApplicantId",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantExperiences_TenantId",
                schema: "Hr",
                table: "ApplicantExperiences");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantExperiences_TenantId_Id",
                schema: "Hr",
                table: "ApplicantExperiences");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantEducations_TenantId",
                schema: "Hr",
                table: "ApplicantEducations");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantEducations_TenantId_Id",
                schema: "Hr",
                table: "ApplicantEducations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "SalaryStructures");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "SalaryStructureComponents");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "RecruitmentStages");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "PayrollRecords");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "PayrollComponents");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "LoanInstallments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "EmployeeContracts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "ApplicantExperiences");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Hr",
                table: "ApplicantEducations");
        }
    }
}
