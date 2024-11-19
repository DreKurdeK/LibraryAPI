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
    IValidator<BookDto> bookValidator
    ) : ControllerBase
{
    private readonly IBookService _bookService = bookService;
    private readonly ILogger<BookController> _logger = logger;
    private readonly IValidator<BookDto> _bookValidator = bookValidator;

    
    // GET: api/book
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksAsync()
    {
        _logger.LogInformation("Fetching all books");
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    // GET: api/book/{id}
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
    
    // POST: api/book
    [HttpPost]
    public async Task<ActionResult<BookDto>> AddAsync(BookDto? bookDto)
    {
        if (bookDto is null) return BadRequest();
        _logger.LogInformation("Adding book: {book}", bookDto);

        var validationResult = await _bookValidator.ValidateAsync(bookDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Book data is invalid");
            return BadRequest(validationResult.Errors);
        }
    
        await _bookService.AddAsync(bookDto);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = bookDto.Id }, bookDto);
    }

    // PUT: api/book/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, BookDto bookDto)
    {
        if (id != bookDto.Id)
        {
            _logger.LogWarning("Book ID in the route does not match ID in the body");
            return BadRequest("ID mismatch");
        }
    
        var validationResult = await _bookValidator.ValidateAsync(bookDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Book data is invalid");
            return BadRequest(validationResult.Errors);
        }
    
        await _bookService.UpdateAsync(bookDto);
        return NoContent();
    }

    
    // DELETE: api/book/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting book with id: {id}", id);
        await _bookService.DeleteAsync(id);
        return NoContent();
    }
}