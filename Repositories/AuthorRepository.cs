using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class AuthorRepository(LibraryDbContext dbContext, IMapper mapper) : IAuthorRepository
{
    private readonly LibraryDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedResult<Author>> GetAllAuthorsAsync(int pageNumber, int pageSize, string sortBy = "LastName", bool ascending = true)
    {
        var query = _dbContext.Authors.AsQueryable();

        // Sorting
        switch (sortBy.ToLower())
        {
            case "lastname":
                query = ascending ? query.OrderBy(p => p.LastName) : query.OrderByDescending(p => p.LastName);
                break;
            case "dateofbirth":
                query = ascending ? query.OrderBy(p => p.DateOfBirth) : query.OrderByDescending(p => p.DateOfBirth);
                break;
            default:
                query = ascending ? query.OrderBy(p => p.FirstName) : query.OrderByDescending(p => p.FirstName);
                break;
        }
        
        // Pagination
        var totalItems = await query.CountAsync();
    
        var authors = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Author>
        {
            TotalItems = totalItems,
            Items = authors
        };
    }


    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Authors.FindAsync(id);
    }

    public async Task AddAsync(AuthorDto authorDto)
    {
        var author = _mapper.Map<Author>(authorDto);
        author.Id = Guid.NewGuid();
        await _dbContext.Authors.AddAsync(author);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(AuthorDto authorDto)
    {
        if (authorDto.Id == Guid.Empty) throw new NullReferenceException("Author Id is required");

        var authorToUpdate = await _dbContext.Authors.FindAsync(authorDto.Id);
        if (authorToUpdate == null)
        {
            throw new AuthorNotFoundException(authorDto.Id);
        }

        _mapper.Map(authorDto, authorToUpdate);

        _dbContext.Authors.Update(authorToUpdate);
        await _dbContext.SaveChangesAsync();
    }


    public async Task DeleteAsync(Guid id)
    {
        var author = await _dbContext.Authors.FindAsync(id);
        if (author != null)
        {
            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();
        }
    }
}