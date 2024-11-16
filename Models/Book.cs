using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Book
{
    public Guid Id { get; set; } // Primary Key
    public string Title { get; set; }
    public string ISBN { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public BookCategory Category { get; set; }
    
    [Required(ErrorMessage = "Author id is required")]
    public Guid AuthorId { get; set; } 
    public Author Author { get; set; } 
    
    [Required(ErrorMessage = "Publisher id is required")]
    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; }
    
    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date, ErrorMessage = "Date must be a valid date")]
    public DateTime ReleaseDate { get; set; }
}