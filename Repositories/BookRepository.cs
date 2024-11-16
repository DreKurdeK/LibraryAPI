using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class BookRepository(LibraryDbContext dbContext, IMapper mapper) : IBookRepository
{
    private readonly LibraryDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _dbContext.Books.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Books.FindAsync(id);
    }

    public async Task AddAsync(BookDTO bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        book.Id = Guid.NewGuid();
        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        var bookUpdated = await _dbContext.Books.FindAsync(book.Id);
        if (bookUpdated != null)
        {
            _mapper.Map(book, bookUpdated);
            _dbContext.Books.Update(bookUpdated);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book != null)
        {
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }
    }
}