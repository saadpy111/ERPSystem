using FluentValidation;
using System.Linq;

namespace Accounting.Application.Features.JournalEntries.Commands.CreateJournalEntry
{
    public class CreateJournalEntryValidator : AbstractValidator<CreateJournalEntryCommand>
    {
        public CreateJournalEntryValidator()
        {
            RuleFor(v => v.Lines)
                .NotEmpty()
                .Must(l => l != null && l.Count >= 2)
                .WithMessage("A journal entry must have at least 2 lines.");

            RuleFor(v => v.Lines).Custom((lines, context) =>
            {
                if (lines == null || lines.Count == 0) return;

                decimal totalDebit = 0;
                decimal totalCredit = 0;

                foreach (var line in lines)
                {
                    if (line.Debit > 0 && line.Credit > 0)
                    {
                        context.AddFailure("Lines", "A line cannot have both Debit and Credit.");
                    }
                    if (line.Debit <= 0 && line.Credit <= 0)
                    {
                        context.AddFailure("Lines", "A line must have either Debit or Credit > 0.");
                    }

                    totalDebit += line.Debit;
                    totalCredit += line.Credit;
                }

                if (totalDebit != totalCredit)
                {
                    context.AddFailure("Lines", "Total Debit must equal Total Credit.");
                }
            });
        }
    }
}
