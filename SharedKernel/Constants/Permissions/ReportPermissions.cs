namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Report Module Permissions
    /// Based on actual endpoints from Report.Api controllers
    /// </summary>
    public static class ReportPermissions
    {
        public const string Module = "Report";

        // ===== REPORTS =====
        public const string ReportsView = "Report.Reports.View";
        public const string ReportsExport = "Report.Reports.Export";

        // ===== INVENTORY REPORTS =====
        public const string InventoryReportsView = "Report.Inventory.View";
        public const string InventoryReportsExport = "Report.Inventory.Export";

        // ===== HR REPORTS =====
        public const string HrReportsView = "Report.HR.View";
        public const string HrReportsExport = "Report.HR.Export";

        // ===== PROCUREMENT REPORTS =====
        public const string ProcurementReportsView = "Report.Procurement.View";
        public const string ProcurementReportsExport = "Report.Procurement.Export";

        // ===== FINANCIAL REPORTS =====
        public const string FinancialReportsView = "Report.Financial.View";
        public const string FinancialReportsExport = "Report.Financial.Export";
    }
}
