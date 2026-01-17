namespace SharedKernel.Constants
{
    /// <summary>
    /// Central repository for all role names used across the system.
    /// Ensures consistency and compile-time safety for role references.
    /// </summary>
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string InventoryManager = "InventoryManager";
        public const string HRManager = "HRManager";
        public const string ProcurementManager = "ProcurementManager";
        public const string ReportViewer = "ReportViewer";
        public const string WebsiteAdmin = "WebsiteManager";
    }
}
