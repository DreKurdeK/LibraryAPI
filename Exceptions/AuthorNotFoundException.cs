namespace LibraryAPI.Exceptions;

public class AuthorNotFoundException : Exception
{
    public Guid AuthorId { get; }

    public AuthorNotFoundException(Guid authorId)
        : base($"Author with ID {authorId} does not exist.")
    {
        AuthorId = authorId;
    }

    public AuthorNotFoundException(Guid authorId, string message)
        : base(message)
    {
        AuthorId = authorId;
    }
}