using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Services;
using LibraryAPI.Validators;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController(
    IAuthorService authorService,
    ILogger<AuthorController> logger,
    IValidator<AuthorDto> authorValidator
    ) : ControllerBase
{
    private readonly IAuthorService _authorService = authorService;
    private readonly ILogger<AuthorController> _logger = logger;
    private readonly IValidator<AuthorDto> _authorValidator = authorValidator;
    
    // GET: api/author
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthorsAsync()
    {
        _logger.LogInformation("Fetching all authors");
        var authors = await _authorService.GetAllAuthorsAsync();
        return Ok(authors);
    }

    // GET: api/author/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching author with id: {id}", id);
        var author = await _authorService.GetByIdAsync(id);
        
        if (author is null)
        {
            _logger.LogInformation("Author with id: {id} not found", id);
            return NotFound();
        }
        return Ok(author);
    }
    
    // POST: api/author
    [HttpPost]
    public async Task<ActionResult> AddAsync(AuthorDto authorDto)
    {
        if (authorDto == null) return BadRequest("Author data is required.");
        
        var validationResult = await _authorValidator.ValidateAsync(authorDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Author data is invalid");
            return BadRequest(validationResult.Errors);
        }

        await _authorService.AddAsync(authorDto);
        return CreatedAtAction(nameof(AddAsync), new { id = authorDto.Id }, authorDto);
    }

    // PUT: api/author/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] AuthorDto authorDto)
    {
        if (id != authorDto.Id)
        {
            _logger.LogWarning("Author ID in the route does not match ID in the body");
            return BadRequest("ID mismatch");
        }

        var validationResult = await _authorValidator.ValidateAsync(authorDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Author data is invalid");
            return BadRequest(validationResult.Errors);
        }

        await _authorService.UpdateAsync(authorDto);
        return NoContent();
    }
    
    // DELETE: api/author/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting author with id: {id}", id);
        await _authorService.DeleteAsync(id);
        return NoContent();
    }
}