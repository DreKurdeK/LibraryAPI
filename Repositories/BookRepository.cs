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
    public async Task<PagedResult<Book>> GetAllBooksAsync(int pageNumber, int pageSize, string sortBy, bool ascending)
    {
        var query = _dbContext.Books.AsQueryable();

        // Key for sorting query
        Dictionary<string, Func<IQueryable<Book>, bool, IOrderedQueryable<Book>>> sortOptions = new Dictionary<string, Func<IQueryable<Book>, bool, IOrderedQueryable<Book>>>
        {
            { "title", (query, ascending) => ascending ? query.OrderBy(p => p.Title) : query.OrderByDescending(p => p.Title) },
            { "author", (query, ascending) => ascending ? query.OrderBy(p => p.Author.LastName) : query.OrderByDescending(p => p.Author.LastName) },
            { "publisher", (query, ascending) => ascending ? query.OrderBy(p => p.Publisher.Name) : query.OrderByDescending(p => p.Publisher.Name) },
            { "category", (query, ascending) => ascending ? query.OrderBy(p => p.Category.ToString()) : query.OrderByDescending(p => p.Category.ToString()) },
            { "releasedate", (query, ascending) => ascending ? query.OrderBy(p => p.ReleaseDate) : query.OrderByDescending(p => p.ReleaseDate) }
        };
        
        // Be case-insensitive
        string sortKey = sortBy.ToLower();
        
        // Sort by key or by default (title)
        if (sortOptions.ContainsKey(sortKey))
        {
            query = sortOptions[sortKey](query, ascending);
        }
        else
        {
            query = ascending ? query.OrderBy(p => p.Title) : query.OrderByDescending(p => p.Title);
        }
        
        // Pagination 
        var totalItems = await query.CountAsync();
        var books = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Book>
        {
            TotalItems = totalItems,
            Items = books
        };
    }


    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Books.FindAsync(id);
    }

    public async Task AddAsync(BookDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        book.Id = Guid.NewGuid();
        
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