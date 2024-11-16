using FluentValidation;
using FluentValidation.Validators;
using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Validators;

public class BookDtoValidator : AbstractValidator<BookDto>
{
    public BookDtoValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(1, 200).WithMessage("Title cannot exceed 200 characters");
        
        RuleFor(b => b.ISBN)
            .NotEmpty().WithMessage("ISBN is required")
            .Matches(@"^\d{13}$").WithMessage("ISBN must be exactly 13 digits");
        
        RuleFor(b => b.Category)
            .IsInEnum().WithMessage("Category is required or incorrect value");

        RuleFor(b => b.AuthorId)
            .NotEmpty().WithMessage("Author Id is required");
        
        RuleFor(b => b.PublisherId)
            .NotEmpty().WithMessage("Publisher Id is required");
        
        RuleFor(b => b.ReleaseDate)
            .NotEmpty().WithMessage("Release Date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Release Date cannot be in the future");
        
    }
}