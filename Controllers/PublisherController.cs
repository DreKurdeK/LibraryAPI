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
    IValidator<PublisherDto> publisherDtoValidator,
    IValidator<Publisher> publisherValidator
    ) : ControllerBase
{
    private readonly IPublisherService _publisherService = publisherService;
    private readonly ILogger<PublisherController> _logger = logger;
    private readonly IValidator<PublisherDto> _publisherDtoValidator = publisherDtoValidator;
    private readonly IValidator<Publisher> _publisherValidator = publisherValidator;
    
    // GET: api/publisher
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Publisher>>> GetAllPublishersAsync()
    {
        _logger.LogInformation("Fetching all publishers");
        var publishers = await _publisherService.GetAllPublishersAsync();
        return Ok(publishers);
    }

    // GET: api/product/{id}
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
    
    // POST: api/product
    [HttpPost]
    public async Task<ActionResult<PublisherDto>> AddAsync(PublisherDto? publisherDto)
    {
        if (publisherDto is null) return BadRequest();
        _logger.LogInformation("Adding publisher: {publisher}", publisherDto);

        var validationResult = await _publisherDtoValidator.ValidateAsync(publisherDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Publisher data is invalid");
            return BadRequest(validationResult.Errors);
        }
        
        await _publisherService.AddAsync(publisherDto);
        return Created();
    }
    
    // PUT: api/product/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, Publisher publisher)
    {
        if (id != publisher.Id)
        {
            _logger.LogWarning("Product ID in the route does not match ID in the body");
            return BadRequest("ID mismatch");
        }
        
        var validationResult = await _publisherValidator.ValidateAsync(publisher);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Product data is invalid");
            return BadRequest(validationResult.Errors);
        }
        
        await _publisherService.UpdateAsync(publisher);
        return NoContent();
    }
    
    // DELETE: api/product/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting publisher with id: {id}", id);
        await _publisherService.DeleteAsync(id);
        return NoContent();
    }
}