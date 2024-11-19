using LibraryAPI.Models;

namespace LibraryAPI.DTOs;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public BookCategory Category { get; set; }
    public Guid AuthorId { get; set; } 
    public Guid PublisherId { get; set; }
    public DateTime ReleaseDate { get; set; }
}