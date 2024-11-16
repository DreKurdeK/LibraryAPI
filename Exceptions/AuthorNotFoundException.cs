namespace LibraryAPI.Exceptions;

public class AuthorNotFoundException : Exception
{
    public int AuthorId { get; }

    public AuthorNotFoundException(Guid authorId)
        : base($"Author with ID {authorId} does not exist.")
    {
        AuthorId = authorId;
    }

    public AuthorNotFoundException(int authorId, string message)
        : base(message)
    {
        AuthorId = authorId;
    }
}