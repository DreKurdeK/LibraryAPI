namespace LibraryAPI.Exceptions;

public class PublisherNotFoundException : Exception
{
    public int PublisherId { get; }

    public PublisherNotFoundException(Guid publisherId)
        : base($"Publisher with ID {publisherId} does not exist.")
    {
        PublisherId = publisherId;
    }

    public PublisherNotFoundException(int publisherId, string message)
        : base(message)
    {
        PublisherId = publisherId;
    }
}