namespace LibraryAPI.Exceptions;

public class UserNotFoundException : Exception
{
    public Guid UserId { get; }

    public UserNotFoundException(Guid userId)
        : base($"User with ID {userId} does not exist.")
    {
        UserId = userId;
    }

    public UserNotFoundException(Guid userId, string message)
        : base(message)
    {
        UserId = userId;
    }
}