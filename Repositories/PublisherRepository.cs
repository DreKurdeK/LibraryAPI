using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class PublisherRepository(LibraryDbContext dbContext, IMapper mapper) : IPublisherRepository
{
    private readonly LibraryDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<List<Publisher>> GetAllPublishersAsync()
    {
        return await _dbContext.Publishers.ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Publishers.FindAsync(id);
    }

    public async Task AddAsync(PublisherDto publisherDto)
    {
        var publisher = _mapper.Map<Publisher>(publisherDto);
        publisher.Id = Guid.NewGuid();
        await _dbContext.Publishers.AddAsync(publisher);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(PublisherDto publisherDto)
    {
        if (publisherDto.Id == Guid.Empty) throw new NullReferenceException("Publisher Id is required"); 

        var publisherToUpdate = await _dbContext.Publishers.FindAsync(publisherDto.Id);
        if (publisherToUpdate == null)
        {
            throw new PublisherNotFoundException(publisherDto.Id);
        }

        _mapper.Map(publisherDto, publisherToUpdate);

        _dbContext.Publishers.Update(publisherToUpdate);
        await _dbContext.SaveChangesAsync();
    }


    public async Task DeleteAsync(Guid id)
    {
        var publisher = await _dbContext.Publishers.FindAsync(id);
        if (publisher != null)
        {
            _dbContext.Publishers.Remove(publisher);
            await _dbContext.SaveChangesAsync();
        }
    }
}