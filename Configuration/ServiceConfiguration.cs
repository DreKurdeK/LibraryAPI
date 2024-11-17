using FluentValidation;
using LibraryAPI.Data;
using LibraryAPI.Helpers;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using LibraryAPI.Validators;
using Microsoft.EntityFrameworkCore;

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
        
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IPublisherService, PublisherService>();

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        
        services.AddControllers();
    }
}