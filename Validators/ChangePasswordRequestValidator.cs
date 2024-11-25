using FluentValidation;
using LibraryAPI.Models;

namespace LibraryAPI.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .Length(6, 100).WithMessage("New password must be between 6 and 100 characters.")
            .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("New password must contain at least one number.")
            .Matches(@"[\W]").WithMessage("New password must contain at least one special character.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.NewPassword).WithMessage("New password and confirmation password do not match.");
    }
}