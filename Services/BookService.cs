using AutoMapper;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IPublisherRepository publisherRepository, IMapper mapper) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IPublisherRepository _publisherRepository = publisherRepository;
    private readonly IMapper _mapper = mapper;

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

    public async Task AddAsync(BookDTO bookDto)
    {
        var author = await _authorRepository.GetByIdAsync(bookDto.AuthorId);
        if (author == null) throw new AuthorNotFoundException(bookDto.AuthorId);
        
        var publisher = await _publisherRepository.GetByIdAsync(bookDto.PublisherId);
        if (publisher == null) throw new PublisherNotFoundException(bookDto.PublisherId);
        
        var book = _mapper.Map<Book>(bookDto);
        book.Author = author;
        book.Publisher = publisher;
        
        await _bookRepository.AddAsync(book);
    }

    public async Task UpdateAsync(Guid id, BookDTO bookDto)
    {
        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook == null)
        {
            throw new BookNotFoundException(id);
        }
        _mapper.Map(bookDto, existingBook);
        
        var author = await _authorRepository.GetByIdAsync(bookDto.AuthorId);
        if (author == null) throw new AuthorNotFoundException(bookDto.AuthorId);
        existingBook.Author = author;
        
        var publisher = await _publisherRepository.GetByIdAsync(bookDto.PublisherId);
        if (publisher == null) throw new PublisherNotFoundException(bookDto.PublisherId);
        existingBook.Publisher = publisher;
        
        await _bookRepository.UpdateAsync(existingBook);
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