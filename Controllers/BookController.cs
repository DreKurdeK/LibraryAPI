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
    public async Task<ActionResult<PagedResult<Book>>> GetAllBooksAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Title", [FromQuery] bool ascending = true)
    {
        _logger.LogInformation("Fetching books from service.");

        try
        {
            // Call service to get paginated and sorted books
            var result = await _bookService.GetAllBooksAsync(pageNumber, pageSize, sortBy, ascending);

            _logger.LogInformation($"Successfully fetched {result.Items.Count()} books.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching books.");
            return StatusCode(500, "An error occurred while fetching books.");
        }
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
    public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] BookDto bookDto)
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