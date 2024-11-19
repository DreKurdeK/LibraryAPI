using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PublisherController(
    IPublisherService publisherService,
    ILogger<PublisherController> logger,
    IValidator<PublisherDto> publisherValidator
    ) : ControllerBase
{
    private readonly IPublisherService _publisherService = publisherService;
    private readonly ILogger<PublisherController> _logger = logger;
    private readonly IValidator<PublisherDto> _publisherValidator = publisherValidator;
    
    // GET: api/publisher
    [HttpGet]
    public async Task<ActionResult<PagedResult<Publisher>>> GetAllPublishersAsync(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string sortBy = "Name", 
        [FromQuery] bool ascending = true)
    {
        _logger.LogInformation("Fetching publishers from service.");

        try
        {
            var result = await _publisherService.GetAllPublishersAsync(pageNumber, pageSize, sortBy, ascending);

            _logger.LogInformation($"Successfully fetched {result.Items.Count()} publishers.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching publishers.");
            return StatusCode(500, "An error occurred while fetching publishers.");
        }
    }


    // GET: api/publisher/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Publisher>> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching publisher with id: {id}", id);
        var publisher = await _publisherService.GetByIdAsync(id);
        
        if (publisher is null)
        {
            _logger.LogInformation("Publisher with id: {id} not found", id);
            return NotFound();
        }
        return Ok(publisher);
    }
    
    // POST: api/publisher
    [HttpPost]
    public async Task<ActionResult> AddAsync(PublisherDto publisherDto)
    {
        if (publisherDto == null) return BadRequest("Publisher data is required.");
        
        var validationResult = await _publisherValidator.ValidateAsync(publisherDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Publisher data is invalid");
            return BadRequest(validationResult.Errors);
        }

        await _publisherService.AddAsync(publisherDto);
        return CreatedAtAction(nameof(AddAsync), new { id = publisherDto.Id }, publisherDto);
    }

    // PUT: api/publisher/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] PublisherDto publisherDto)
    {
        if (id != publisherDto.Id)
        {
            _logger.LogWarning("Publisher ID in the route does not match ID in the body");
            return BadRequest("ID mismatch");
        }

        var validationResult = await _publisherValidator.ValidateAsync(publisherDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Publisher data is invalid");
            return BadRequest(validationResult.Errors);
        }

        await _publisherService.UpdateAsync(publisherDto);
        return NoContent();
    }
    
    // DELETE: api/publisher/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting publisher with id: {id}", id);
        await _publisherService.DeleteAsync(id);
        return NoContent();
    }
}