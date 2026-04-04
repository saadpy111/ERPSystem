using FluentValidation;

namespace Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry
{
    public class ReverseJournalEntryValidator : AbstractValidator<ReverseJournalEntryCommand>
    {
        public ReverseJournalEntryValidator()
        {
            RuleFor(x => x.JournalEntryId)
                .GreaterThan(0)
                .WithMessage("A valid Journal Entry ID is required.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("A reversal reason is required.")
                .MaximumLength(500)
                .WithMessage("Reversal reason must not exceed 500 characters.");
        }
    }
}
