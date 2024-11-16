namespace LibraryAPI.Exceptions;

public class BookNotFoundException : Exception
{
    public Guid BookId { get; }

    public BookNotFoundException(Guid bookId) 
        : base($"Book with id {bookId} does not exist.")
    {
        BookId = bookId;
    }
    
    public BookNotFoundException(Guid bookId, string message)
        : base(message)
    {
        BookId = bookId;
    }
}