using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Validators;

public class PublisherDtoValidator : AbstractValidator<PublisherDto>
{
    public PublisherDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Publisher name cannot be empty.")
            .Length(1, 200).WithMessage("Publisher name must be between 1 and 200 characters.");

        RuleFor(p => p.Address)
            .MaximumLength(500).WithMessage("Adress cannot be longer than 500 characters.");

        RuleFor(p => p.FoundedYear)
            .InclusiveBetween(1800, DateTime.Now.Year).WithMessage($"Date of year must be between 1800 and {DateTime.Now.Year}.");
    }
}