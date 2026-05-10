using System;

namespace Accounting.Application.Reports.Models.Columns
{
    public abstract class ReportColumn<TItem>
    {
        public string HeaderText { get; set; } = string.Empty;
        public Func<TItem, object?> ValueSelector { get; set; } = _ => null;
        public bool IsVisible { get; set; } = true;
    }

    public class TextColumn<TItem> : ReportColumn<TItem> { }

    public class HierarchicalLabelColumn<TItem> : ReportColumn<TItem>
    {
        public Func<TItem, int> IndentLevelSelector { get; set; } = _ => 0;
    }

    public class DecimalColumn<TItem> : ReportColumn<TItem>
    {
        public string Format { get; set; } = "N2";
    }

    public class DebitColumn<TItem> : ReportColumn<TItem>
    {
        public Func<TItem, Domain.Enums.AccountType> AccountTypeSelector { get; set; } = _ => Domain.Enums.AccountType.Asset;
    }

    public class CreditColumn<TItem> : ReportColumn<TItem>
    {
        public Func<TItem, Domain.Enums.AccountType> AccountTypeSelector { get; set; } = _ => Domain.Enums.AccountType.Asset;
    }

    public class CurrencyColumn<TItem> : ReportColumn<TItem>
    {
        public Func<TItem, string?> CurrencyCodeSelector { get; set; } = _ => null;
    }

    public class PercentageColumn<TItem> : ReportColumn<TItem> { }
}
