using FluentValidation;
using Hr.Application.Features.AttendanceRecordFeatures.DeleteAttendanceRecord;

namespace Hr.Application.Validators
{
    public class DeleteAttendanceRecordValidator : AbstractValidator<DeleteAttendanceRecordRequest>
    {
        public DeleteAttendanceRecordValidator()
        {
            RuleFor(x => x.RecordId)
                .GreaterThan(0).WithMessage("Record ID is required");
        }
    }
}