using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Book
{
    public Guid Id { get; set; } // Primary Key
    public string Title { get; set; }
    public string ISBN { get; set; }
    public BookCategory Category { get; set; }
    public Guid AuthorId { get; set; } 
    public Author Author { get; set; } 
    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; }
    public DateTime ReleaseDate { get; set; }
}