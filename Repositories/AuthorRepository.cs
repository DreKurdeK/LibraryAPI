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

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _dbContext.Authors.ToListAsync();
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