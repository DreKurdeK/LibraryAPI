using LibraryAPI.Exceptions;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public class AuthorService(IAuthorService authorService) : IAuthorService
{
    private readonly IAuthorService _authorService = authorService;

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null)
        {
            throw new AuthorNotFoundException(id);
        }
        
        return author!;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _authorService.GetAllAuthorsAsync();
    }

    public async Task AddAsync(Author author)
    {
        
    }

    public async Task UpdateAsync(Author author)
    {
        var existingAuthor = await _authorService.GetByIdAsync(author.Id);
        if (existingAuthor == null)
        {
            throw new AuthorNotFoundException(author.Id);
        }
        await _authorService.UpdateAsync(author);
    }

    public async Task DeleteAsync(Guid id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null)
        {
            throw new AuthorNotFoundException(id);
        }

        await _authorService.DeleteAsync(id);
    }
}