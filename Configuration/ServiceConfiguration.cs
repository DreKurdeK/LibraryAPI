using System.Diagnostics.Eventing.Reader;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryAPI.Data;
using LibraryAPI.Helpers;
using LibraryAPI.Repositories;
using LibraryAPI.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
            logging.AddEventSourceLogger();
        });
        
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        
        services.AddValidatorsFromAssemblyContaining<BookValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthorValidator>();
        services.AddValidatorsFromAssemblyContaining<PublisherValidator>();
        
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        
    }
}