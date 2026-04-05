namespace SharedKernel.Constants.Permissions
{
    public static class AccountingPermissions
    {
        public const string Module = "Accounting";

        // ===== ACCOUNTS =====
        public const string AccountsView = "Permissions.Accounting.Accounts.View";
        public const string AccountsCreate = "Permissions.Accounting.Accounts.Create";
        public const string AccountsEdit = "Permissions.Accounting.Accounts.Edit";

        // ===== JOURNAL ENTRIES =====
        public const string JournalEntriesView = "Permissions.Accounting.JournalEntries.View";
        public const string JournalEntriesCreate = "Permissions.Accounting.JournalEntries.Create";
        public const string JournalEntriesPost = "Permissions.Accounting.JournalEntries.Post";
        public const string JournalEntriesReverse = "Permissions.Accounting.JournalEntries.Reverse";

        // ===== POSTING =====
        public const string PostingExecute = "Permissions.Accounting.Posting.Execute";

        // ===== REPORTS =====
        public const string ReportsTrialBalance = "Permissions.Accounting.Reports.TrialBalance";
        public const string ReportsGeneralLedger = "Permissions.Accounting.Reports.GeneralLedger";
        public const string ReportsAccountStatement = "Permissions.Accounting.Reports.AccountStatement";
        public const string ReportsBalanceSheet = "Permissions.Accounting.Reports.BalanceSheet";
        public const string ReportsIncomeStatement = "Permissions.Accounting.Reports.IncomeStatement";

        // ===== PARTNERS =====
        public const string PartnersView = "Permissions.Accounting.Partners.View";
        public const string PartnersCreate = "Permissions.Accounting.Partners.Create";

        // ===== FISCAL =====
        public const string FiscalView = "Permissions.Accounting.Fiscal.View";
        public const string FiscalManage = "Permissions.Accounting.Fiscal.Manage";

        // ===== COST CENTERS =====
        public const string CostCentersView = "Permissions.Accounting.CostCenters.View";
        public const string CostCentersCreate = "Permissions.Accounting.CostCenters.Create";

        // ===== CASH =====
        public const string CashView = "Permissions.Accounting.Cash.View";
        public const string CashCreate = "Permissions.Accounting.Cash.Create";
    }
}
