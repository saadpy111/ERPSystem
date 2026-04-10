namespace SharedKernel.Core.Constants.Permissions
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

        // ===== VOUCHERS =====
        public const string VouchersView = "Permissions.Accounting.Vouchers.View";
        public const string VouchersCreate = "Permissions.Accounting.Vouchers.Create";
        public const string VouchersApprove = "Permissions.Accounting.Vouchers.Approve";
        public const string VouchersPost = "Permissions.Accounting.Vouchers.Post";

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
        public const string FiscalView   = "Permissions.Accounting.Fiscal.View";
        public const string FiscalManage = "Permissions.Accounting.Fiscal.Manage";
        public const string FiscalCreate = "Permissions.Accounting.Fiscal.Create";
        public const string FiscalEdit   = "Permissions.Accounting.Fiscal.Edit";
        public const string FiscalDelete = "Permissions.Accounting.Fiscal.Delete";


        // ===== COST CENTERS =====
        public const string CostCentersView = "Permissions.Accounting.CostCenters.View";
        public const string CostCentersCreate = "Permissions.Accounting.CostCenters.Create";

        // ===== CASH =====
        public const string CashView = "Permissions.Accounting.Cash.View";
        public const string CashCreate = "Permissions.Accounting.Cash.Create";

        // ===== CURRENCIES =====
        public const string CurrenciesView = "Permissions.Accounting.Currencies.View";
        public const string CurrenciesCreate = "Permissions.Accounting.Currencies.Create";
        public const string CurrenciesEdit = "Permissions.Accounting.Currencies.Edit";

        // ===== CURRENCY RATES =====
        public const string CurrencyRatesView = "Permissions.Accounting.CurrencyRates.View";
        public const string CurrencyRatesCreate = "Permissions.Accounting.CurrencyRates.Create";

        // ===== ACCOUNTING MAPPINGS =====
        public const string AccountingMappingsView = "Permissions.Accounting.AccountingMappings.View";
        public const string AccountingMappingsCreate = "Permissions.Accounting.AccountingMappings.Create";
        public const string AccountingMappingsEdit = "Permissions.Accounting.AccountingMappings.Edit";
        public const string AccountingMappingsDelete = "Permissions.Accounting.AccountingMappings.Delete";

        // ===== CASH ACCOUNTS =====
        public const string CashAccountsView = "Permissions.Accounting.CashAccounts.View";
        public const string CashAccountsCreate = "Permissions.Accounting.CashAccounts.Create";
        public const string CashAccountsEdit = "Permissions.Accounting.CashAccounts.Edit";
        public const string CashAccountsDelete = "Permissions.Accounting.CashAccounts.Delete";

        // ===== RECEIVABLES =====
        public const string ReceivablesView = "Permissions.Accounting.Receivables.View";
        public const string ReceivablesCreate = "Permissions.Accounting.Receivables.Create";
        public const string ReceivablesPay = "Permissions.Accounting.Receivables.Pay";

        // ===== PAYABLES =====
        public const string PayablesView = "Permissions.Accounting.Payables.View";
        public const string PayablesCreate = "Permissions.Accounting.Payables.Create";
        public const string PayablesPay = "Permissions.Accounting.Payables.Pay";
    }
}
