using Accounting.Application.Reports.Models.Columns;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Reports.Builders
{
    public class ReportDefinition<TItem>
    {
        public List<ReportColumn<TItem>> Columns { get; } = new();
        public bool IncludeTotals { get; set; } = true;
    }

    public class ReportDefinitionBuilder<TItem>
    {
        private readonly ReportDefinition<TItem> _report = new();

        public ReportDefinitionBuilder<TItem> AddTextColumn(Func<TItem, object?> valueSelector, string header)
        {
            _report.Columns.Add(new TextColumn<TItem> { ValueSelector = valueSelector, HeaderText = header });
            return this;
        }

        public ReportDefinitionBuilder<TItem> AddHierarchicalLabelColumn(Func<TItem, object?> valueSelector, string header, Func<TItem, int> indentLevelSelector)
        {
            _report.Columns.Add(new HierarchicalLabelColumn<TItem> { ValueSelector = valueSelector, HeaderText = header, IndentLevelSelector = indentLevelSelector });
            return this;
        }

        public ReportDefinitionBuilder<TItem> AddDecimalColumn(Func<TItem, object?> valueSelector, string header, string format = "N2")
        {
            _report.Columns.Add(new DecimalColumn<TItem> { ValueSelector = valueSelector, HeaderText = header, Format = format });
            return this;
        }

        public ReportDefinitionBuilder<TItem> AddDebitColumn(Func<TItem, object?> valueSelector, string header, Func<TItem, Domain.Enums.AccountType> accountTypeSelector)
        {
            _report.Columns.Add(new DebitColumn<TItem> { ValueSelector = valueSelector, HeaderText = header, AccountTypeSelector = accountTypeSelector });
            return this;
        }

        public ReportDefinitionBuilder<TItem> AddCreditColumn(Func<TItem, object?> valueSelector, string header, Func<TItem, Domain.Enums.AccountType> accountTypeSelector)
        {
            _report.Columns.Add(new CreditColumn<TItem> { ValueSelector = valueSelector, HeaderText = header, AccountTypeSelector = accountTypeSelector });
            return this;
        }

        public ReportDefinitionBuilder<TItem> AddCurrencyColumn(Func<TItem, object?> valueSelector, string header, Func<TItem, string?> currencyCodeSelector)
        {
            _report.Columns.Add(new CurrencyColumn<TItem> { ValueSelector = valueSelector, HeaderText = header, CurrencyCodeSelector = currencyCodeSelector });
            return this;
        }

        public ReportDefinitionBuilder<TItem> AddPercentageColumn(Func<TItem, object?> valueSelector, string header)
        {
            _report.Columns.Add(new PercentageColumn<TItem> { ValueSelector = valueSelector, HeaderText = header });
            return this;
        }

        public ReportDefinitionBuilder<TItem> HasTotals(bool includeTotals = true)
        {
            _report.IncludeTotals = includeTotals;
            return this;
        }

        public ReportDefinition<TItem> Build()
        {
            return _report;
        }
    }
}
