using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Author
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date, ErrorMessage = "Date of Birth must be a date")]
    public DateTime DateOfBirth { get; set; }
}