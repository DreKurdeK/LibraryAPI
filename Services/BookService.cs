using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllBooksAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        
        return book;
    }

    public async Task AddAsync(Book book)
    {
        
    }

    public async Task UpdateAsync(Book book)
    {
        var existingBook = await _bookRepository.GetByIdAsync(book.Id);
        if (existingBook == null)
        {
            throw new BookNotFoundException(book.Id);
        }
        
        await _bookRepository.UpdateAsync(book);
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            throw new BookNotFoundException(id);
        }
        
        await _bookRepository.DeleteAsync(id);
    }
}