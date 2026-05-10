using Accounting.Domain.Enums;
using System;
using System.Globalization;
using System.Threading;

namespace Accounting.Application.Reports.Services
{
    public class FinancialFormattingService : IFinancialFormattingService
    {
        public string FormatCurrency(decimal amount, string? currencyCode = null)
        {
            // Fallback to culture default if no currency code is provided
            var cultureInfo = CultureInfo.CurrentCulture;
            
            // Format example: 260,900.00
            if (string.IsNullOrEmpty(currencyCode))
            {
                return amount.ToString("N2", cultureInfo);
            }
            
            return $"{amount.ToString("N2", cultureInfo)} {currencyCode}";
        }

        public string FormatDecimal(decimal value, int precision = 2)
        {
            return value.ToString($"N{precision}", CultureInfo.CurrentCulture);
        }

        public string FormatPercentage(decimal value)
        {
            return (value / 100m).ToString("P2", CultureInfo.CurrentCulture);
        }

        public decimal GetCreditBalance(decimal balance, AccountType accountType)
        {
            // If balance > 0 for Liabilities/Equity/Revenue, it's a credit
            // If balance < 0 for Assets/Expenses, it's a credit
            if (accountType == AccountType.Liability || accountType == AccountType.Equity || accountType == AccountType.Revenue)
            {
                return balance > 0 ? balance : 0m;
            }
            else // Asset, Expense
            {
                return balance < 0 ? Math.Abs(balance) : 0m;
            }
        }

        public decimal GetDebitBalance(decimal balance, AccountType accountType)
        {
            // If balance > 0 for Assets/Expenses, it's a debit
            // If balance < 0 for Liabilities/Equity/Revenue, it's a debit
            if (accountType == AccountType.Asset || accountType == AccountType.Expense)
            {
                return balance > 0 ? balance : 0m;
            }
            else // Liability, Equity, Revenue
            {
                return balance < 0 ? Math.Abs(balance) : 0m;
            }
        }

        public decimal GetNaturalBalance(decimal balance, AccountType accountType)
        {
            // Positive represents the natural side
            if (accountType == AccountType.Liability || accountType == AccountType.Equity || accountType == AccountType.Revenue)
            {
                return balance; // Credit is natural
            }
            
            return balance; // Asset/Expense Debit is natural
        }

        public bool IsArabicCulture()
        {
            var cultureName = CultureInfo.CurrentCulture.Name;
            return cultureName.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
        }
    }
}
