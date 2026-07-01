namespace SharedKernel.Core.Constants.Permissions
{
    public static class AccountingPermissions
    {
        public const string Module = "Accounting";

        // ===== ACCOUNTS =====
        public const string AccountsView = "Accounting.Accounts.View";
        public const string AccountsCreate = "Accounting.Accounts.Create";
        public const string AccountsEdit = "Accounting.Accounts.Edit";

        // ===== JOURNAL ENTRIES =====
        public const string JournalEntriesView = "Accounting.JournalEntries.View";
        public const string JournalEntriesCreate = "Accounting.JournalEntries.Create";
        public const string JournalEntriesPost = "Accounting.JournalEntries.Post";
        public const string JournalEntriesReverse = "Accounting.JournalEntries.Reverse";

        // ===== VOUCHERS =====
        public const string VouchersView = "Accounting.Vouchers.View";
        public const string VouchersCreate = "Accounting.Vouchers.Create";
        public const string VouchersApprove = "Accounting.Vouchers.Approve";
        public const string VouchersPost = "Accounting.Vouchers.Post";

        // ===== POSTING =====
        public const string PostingExecute = "Accounting.Posting.Execute";

        // ===== REPORTS =====
        public const string ReportsTrialBalance = "Accounting.Reports.TrialBalance";
        public const string ReportsGeneralLedger = "Accounting.Reports.GeneralLedger";
        public const string ReportsAccountStatement = "Accounting.Reports.AccountStatement";
        public const string ReportsBalanceSheet = "Accounting.Reports.BalanceSheet";
        public const string ReportsIncomeStatement = "Accounting.Reports.IncomeStatement";
        public const string ReportsBudgetVsActual = "Accounting.Reports.BudgetVsActual";
        public const string ReportsExpenseAnalysis = "Accounting.Reports.ExpenseAnalysis";
        public const string ReportsProfitability = "Accounting.Reports.Profitability";

        // ===== DASHBOARD =====
        public const string DashboardView = "Accounting.Dashboard.View";

        // ===== PARTNERS =====
        public const string PartnersView = "Accounting.Partners.View";
        public const string PartnersCreate = "Accounting.Partners.Create";
        public const string PartnersEdit = "Accounting.Partners.Edit";
        public const string PartnersDelete = "Accounting.Partners.Delete";

        // ===== FISCAL =====
        public const string FiscalView = "Accounting.Fiscal.View";
        public const string FiscalManage = "Accounting.Fiscal.Manage";
        public const string FiscalCreate = "Accounting.Fiscal.Create";
        public const string FiscalEdit = "Accounting.Fiscal.Edit";
        public const string FiscalDelete = "Accounting.Fiscal.Delete";


        // ===== COST CENTERS =====
        public const string CostCentersView = "Accounting.CostCenters.View";
        public const string CostCentersCreate = "Accounting.CostCenters.Create";
        public const string CostCentersEdit = "Accounting.CostCenters.Edit";
        public const string CostCentersDelete = "Accounting.CostCenters.Delete";

        // ===== CASH =====
        public const string CashView = "Accounting.Cash.View";
        public const string CashTransactionsView = "Accounting.CashTransactions.View";
        public const string CashCreate = "Accounting.Cash.Create";

        // ===== CURRENCIES =====
        public const string CurrenciesView = "Accounting.Currencies.View";
        public const string CurrenciesCreate = "Accounting.Currencies.Create";
        public const string CurrenciesEdit = "Accounting.Currencies.Edit";

        // ===== CURRENCY RATES =====
        public const string CurrencyRatesView = "Accounting.CurrencyRates.View";
        public const string CurrencyRatesCreate = "Accounting.CurrencyRates.Create";

        // ===== ACCOUNTING MAPPINGS =====
        public const string AccountingMappingsView = "Accounting.AccountingMappings.View";
        public const string AccountingMappingsCreate = "Accounting.AccountingMappings.Create";
        public const string AccountingMappingsEdit = "Accounting.AccountingMappings.Edit";
        public const string AccountingMappingsDelete = "Accounting.AccountingMappings.Delete";

        // ===== CASH ACCOUNTS =====
        public const string CashAccountsView = "Accounting.CashAccounts.View";
        public const string CashAccountsCreate = "Accounting.CashAccounts.Create";
        public const string CashAccountsEdit = "Accounting.CashAccounts.Edit";
        public const string CashAccountsDelete = "Accounting.CashAccounts.Delete";

        // ===== RECEIVABLES =====
        public const string ReceivablesView = "Accounting.Receivables.View";
        public const string ReceivablesCreate = "Accounting.Receivables.Create";
        public const string ReceivablesPay = "Accounting.Receivables.Pay";

        // ===== PAYABLES =====
        public const string PayablesView = "Accounting.Payables.View";
        public const string PayablesCreate = "Accounting.Payables.Create";
        public const string PayablesPay = "Accounting.Payables.Pay";

        // ===== BUDGETS =====
        public const string BudgetsView = "Accounting.Budgets.View";
        public const string BudgetsCreate = "Accounting.Budgets.Create";
        public const string BudgetsEdit = "Accounting.Budgets.Edit";
        public const string BudgetsDelete = "Accounting.Budgets.Delete";
        public const string BudgetsReport = "Accounting.Budgets.Report";
    }
}

