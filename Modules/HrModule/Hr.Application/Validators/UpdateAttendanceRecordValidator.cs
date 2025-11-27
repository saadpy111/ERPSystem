using FluentValidation;
using Hr.Application.Features.AttendanceRecordFeatures.UpdateAttendanceRecord;

namespace Hr.Application.Validators
{
    public class UpdateAttendanceRecordValidator : AbstractValidator<UpdateAttendanceRecordRequest>
    {
        public UpdateAttendanceRecordValidator()
        {
            RuleFor(x => x.RecordId)
                .GreaterThan(0).WithMessage("Record ID is required");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee is required");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

            RuleFor(x => x.CheckInTime)
                .NotEmpty().WithMessage("Check-in time is required");

            RuleFor(x => x.CheckOutTime)
                .GreaterThan(x => x.CheckInTime).WithMessage("Check-out time must be after check-in time")
                .When(x => x.CheckOutTime.HasValue);

            RuleFor(x => x.DelayMinutes)
                .GreaterThanOrEqualTo(0).WithMessage("Delay minutes cannot be negative");
        }
    }
}