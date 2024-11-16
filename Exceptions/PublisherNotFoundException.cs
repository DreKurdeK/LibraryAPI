namespace LibraryAPI.Exceptions;

public class PublisherNotFoundException : Exception
{
    public Guid PublisherId { get; }

    public PublisherNotFoundException(Guid publisherId)
        : base($"Publisher with ID {publisherId} does not exist.")
    {
        PublisherId = publisherId;
    }

    public PublisherNotFoundException(Guid publisherId, string message)
        : base(message)
    {
        PublisherId = publisherId;
    }
}