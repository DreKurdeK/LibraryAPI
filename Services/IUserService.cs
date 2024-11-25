using LibraryAPI.Models;
using Microsoft.Identity.Client;

namespace LibraryAPI.Services;

public interface IUserService
{
    Task<User> RegisterAsync(RegisterRequest request);
    Task<string> LoginAsync(LoginRequest request);
}