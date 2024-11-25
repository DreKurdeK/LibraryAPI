using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Services;
using LibraryAPI.Validators;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<Author>>> GetAllAuthorsAsync(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string sortBy = "LastName", 
        [FromQuery] bool ascending = true)
    {
        _logger.LogInformation("Fetching authors from service.");

        try
        {
            var result = await _authorService.GetAllAuthorsAsync(pageNumber, pageSize, sortBy, ascending);

            _logger.LogInformation($"Successfully fetched {result.Items.Count()} authors.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching authors.");
            return StatusCode(500, "An error occurred while fetching authors.");
        }
    }

    // GET: api/author/{id}
    [AllowAnonymous]
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
    [Authorize(Roles = "Manager, Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting author with id: {id}", id);
        await _authorService.DeleteAsync(id);
        return NoContent();
    }
}