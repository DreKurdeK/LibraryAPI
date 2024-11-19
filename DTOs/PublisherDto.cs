namespace LibraryAPI.DTOs;

public class PublisherDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public int FoundedYear { get; set; }
}