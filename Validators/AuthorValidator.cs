using FluentValidation;
using LibraryAPI.Models;

namespace LibraryAPI.Validators;

public class AuthorValidator : AbstractValidator<Author>
{
    public AuthorValidator()
    {
        RuleFor(a => a.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .Length(1, 100).WithMessage("First name must be between 1 and 100 characters.");

        RuleFor(a => a.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .Length(1, 100).WithMessage("Last name must be between 1 and 100 characters.");

        RuleFor(a => a.DateOfBirth)
            .NotEqual(DateTime.MinValue).WithMessage("Birth date cannot be empty.")
            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future.");
    }
}