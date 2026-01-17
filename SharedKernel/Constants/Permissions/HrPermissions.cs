namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// HR Module Permissions
    /// Based on actual endpoints from Hr.Api controllers
    /// </summary>
    public static class HrPermissions
    {
        public const string Module = "HR";

        // ===== EMPLOYEES =====
        public const string EmployeesView = "HR.Employees.View";
        public const string EmployeesCreate = "HR.Employees.Create";
        public const string EmployeesEdit = "HR.Employees.Edit";
        public const string EmployeesDelete = "HR.Employees.Delete";
        public const string EmployeesActivate = "HR.Employees.Activate";
        public const string EmployeesTerminate = "HR.Employees.Terminate";
        public const string EmployeesPromote = "HR.Employees.Promote";
        public const string EmployeesViewSalary = "HR.Employees.ViewSalary";
        public const string EmployeesManageAttachments = "HR.Employees.ManageAttachments";

        // ===== DEPARTMENTS =====
        public const string DepartmentsView = "HR.Departments.View";
        public const string DepartmentsCreate = "HR.Departments.Create";
        public const string DepartmentsEdit = "HR.Departments.Edit";
        public const string DepartmentsDelete = "HR.Departments.Delete";
        public const string DepartmentsViewTree = "HR.Departments.ViewTree";
        public const string DepartmentsManageAttachments = "HR.Departments.ManageAttachments";

        // ===== JOBS =====
        public const string JobsView = "HR.Jobs.View";
        public const string JobsCreate = "HR.Jobs.Create";
        public const string JobsEdit = "HR.Jobs.Edit";
        public const string JobsDelete = "HR.Jobs.Delete";

        // ===== EMPLOYEE CONTRACTS =====
        public const string ContractsView = "HR.Contracts.View";
        public const string ContractsCreate = "HR.Contracts.Create";
        public const string ContractsEdit = "HR.Contracts.Edit";
        public const string ContractsDelete = "HR.Contracts.Delete";

        // ===== ATTENDANCE RECORDS =====
        public const string AttendanceView = "HR.Attendance.View";
        public const string AttendanceCreate = "HR.Attendance.Create";
        public const string AttendanceEdit = "HR.Attendance.Edit";
        public const string AttendanceDelete = "HR.Attendance.Delete";

        // ===== LEAVE TYPES =====
        public const string LeaveTypesView = "HR.LeaveTypes.View";
        public const string LeaveTypesCreate = "HR.LeaveTypes.Create";
        public const string LeaveTypesEdit = "HR.LeaveTypes.Edit";
        public const string LeaveTypesDelete = "HR.LeaveTypes.Delete";

        // ===== LEAVE REQUESTS =====
        public const string LeaveRequestsView = "HR.LeaveRequests.View";
        public const string LeaveRequestsCreate = "HR.LeaveRequests.Create";
        public const string LeaveRequestsEdit = "HR.LeaveRequests.Edit";
        public const string LeaveRequestsDelete = "HR.LeaveRequests.Delete";
        public const string LeaveRequestsApprove = "HR.LeaveRequests.Approve";
        public const string LeaveRequestsReject = "HR.LeaveRequests.Reject";

        // ===== SALARY STRUCTURES =====
        public const string SalaryStructuresView = "HR.SalaryStructures.View";
        public const string SalaryStructuresCreate = "HR.SalaryStructures.Create";
        public const string SalaryStructuresEdit = "HR.SalaryStructures.Edit";
        public const string SalaryStructuresDelete = "HR.SalaryStructures.Delete";

        // ===== SALARY STRUCTURE COMPONENTS =====
        public const string SalaryComponentsView = "HR.SalaryComponents.View";
        public const string SalaryComponentsCreate = "HR.SalaryComponents.Create";
        public const string SalaryComponentsEdit = "HR.SalaryComponents.Edit";
        public const string SalaryComponentsDelete = "HR.SalaryComponents.Delete";

        // ===== PAYROLL COMPONENTS =====
        public const string PayrollComponentsView = "HR.PayrollComponents.View";
        public const string PayrollComponentsCreate = "HR.PayrollComponents.Create";
        public const string PayrollComponentsEdit = "HR.PayrollComponents.Edit";
        public const string PayrollComponentsDelete = "HR.PayrollComponents.Delete";

        // ===== PAYROLL RECORDS =====
        public const string PayrollRecordsView = "HR.PayrollRecords.View";
        public const string PayrollRecordsCreate = "HR.PayrollRecords.Create";
        public const string PayrollRecordsEdit = "HR.PayrollRecords.Edit";
        public const string PayrollRecordsDelete = "HR.PayrollRecords.Delete";
        public const string PayrollRecordsProcess = "HR.PayrollRecords.Process";

        // ===== LOANS =====
        public const string LoansView = "HR.Loans.View";
        public const string LoansCreate = "HR.Loans.Create";
        public const string LoansEdit = "HR.Loans.Edit";
        public const string LoansDelete = "HR.Loans.Delete";
        public const string LoansApprove = "HR.Loans.Approve";
        public const string LoansReject = "HR.Loans.Reject";

        // ===== LOAN INSTALLMENTS =====
        public const string LoanInstallmentsView = "HR.LoanInstallments.View";
        public const string LoanInstallmentsEdit = "HR.LoanInstallments.Edit";

        // ===== RECRUITMENT STAGES =====
        public const string RecruitmentStagesView = "HR.RecruitmentStages.View";
        public const string RecruitmentStagesCreate = "HR.RecruitmentStages.Create";
        public const string RecruitmentStagesEdit = "HR.RecruitmentStages.Edit";
        public const string RecruitmentStagesDelete = "HR.RecruitmentStages.Delete";

        // ===== APPLICANTS =====
        public const string ApplicantsView = "HR.Applicants.View";
        public const string ApplicantsCreate = "HR.Applicants.Create";
        public const string ApplicantsEdit = "HR.Applicants.Edit";
        public const string ApplicantsDelete = "HR.Applicants.Delete";
        public const string ApplicantsHire = "HR.Applicants.Hire";
    }
}
