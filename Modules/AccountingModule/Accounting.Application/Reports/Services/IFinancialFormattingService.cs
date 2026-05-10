using Accounting.Domain.Enums;

namespace Accounting.Application.Reports.Services
{
    public interface IFinancialFormattingService
    {
        string FormatCurrency(decimal amount, string? currencyCode = null);
        string FormatPercentage(decimal value);
        string FormatDecimal(decimal value, int precision = 2);
        
        decimal GetNaturalBalance(decimal balance, AccountType accountType);
        decimal GetDebitBalance(decimal balance, AccountType accountType);
        decimal GetCreditBalance(decimal balance, AccountType accountType);
        
        bool IsArabicCulture();
    }
}
