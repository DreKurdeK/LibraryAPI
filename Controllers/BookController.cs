using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController(
    IBookService bookService,
    ILogger<BookController> logger,
    IValidator<BookDto> bookDtoValidator,
    IValidator<Book> bookValidator
    ) : ControllerBase
{
    private readonly IBookService _bookService = bookService;
    private readonly ILogger<BookController> _logger = logger;
    private readonly IValidator<BookDto> _bookDtoValidator = bookDtoValidator;
    private readonly IValidator<Book> _bookValidator = bookValidator;
    
    // GET: api/book
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksAsync()
    {
        _logger.LogInformation("Fetching all books");
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    // GET: api/product/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching book with id: {id}", id);
        var book = await _bookService.GetByIdAsync(id);
        
        if (book is null)
        {
            _logger.LogInformation("Book with id: {id} not found", id);
            return NotFound();
        }
        return Ok(book);
    }
    
    // POST: api/product
    [HttpPost]
    public async Task<ActionResult<BookDto>> AddAsync(BookDto? bookDto)
    {
        if (bookDto is null) return BadRequest();
        _logger.LogInformation("Adding book: {book}", bookDto);

        var validationResult = await _bookDtoValidator.ValidateAsync(bookDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Book data is invalid");
            return BadRequest(validationResult.Errors);
        }
        
        await _bookService.AddAsync(bookDto);
        return Created();
    }
    
    // PUT: api/product/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, Book book)
    {
        if (id != book.Id)
        {
            _logger.LogWarning("Product ID in the route does not match ID in the body");
            return BadRequest("ID mismatch");
        }
        
        var validationResult = await _bookValidator.ValidateAsync(book);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Product data is invalid");
            return BadRequest(validationResult.Errors);
        }
        
        await _bookService.UpdateAsync(book);
        return NoContent();
    }
    
    // DELETE: api/product/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting book with id: {id}", id);
        await _bookService.DeleteAsync(id);
        return NoContent();
    }
}