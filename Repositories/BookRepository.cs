using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
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

    public async Task AddAsync(BookDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        book.Id = Guid.NewGuid();
    
        // Pobranie Author i Publisher na podstawie Id
        var author = await _dbContext.Authors.FindAsync(bookDto.AuthorId);
        var publisher = await _dbContext.Publishers.FindAsync(bookDto.PublisherId);
    
        if (author == null || publisher == null)
        {
            throw new Exception("Author or Publisher not found");
        }

        book.Author = author;
        book.Publisher = publisher;

        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(BookDto bookDto)
    {
        if (bookDto.Id == Guid.Empty) throw new NullReferenceException("Book Id is required"); 
        
        var bookToUpdate = await _dbContext.Books.FindAsync(bookDto.Id);
        if (bookToUpdate == null)
        {
            throw new BookNotFoundException(bookDto.Id);
        }
        
        _mapper.Map(bookDto, bookToUpdate);
        
        var author = await _dbContext.Authors.FindAsync(bookDto.AuthorId);
        var publisher = await _dbContext.Publishers.FindAsync(bookDto.PublisherId);

        if (author == null) throw new AuthorNotFoundException(bookDto.AuthorId);
        if (publisher == null) throw new PublisherNotFoundException(bookDto.PublisherId);

        bookToUpdate.Author = author;
        bookToUpdate.Publisher = publisher;

        _dbContext.Books.Update(bookToUpdate);
        await _dbContext.SaveChangesAsync();
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