using System.Diagnostics.Eventing.Reader;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryAPI.Data;
using LibraryAPI.Repositories;
using LibraryAPI.Validators;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var conntectionString = configuration["ConnectionStrings:DefaultConnection"];
        
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(conntectionString));
        
        services.AddValidatorsFromAssemblyContaining<BookValidator>();
        
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
    }
}