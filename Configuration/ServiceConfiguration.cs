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
    public static async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        using (var serviceScope = services.BuildServiceProvider().CreateScope())
        {
            var serviceProvider = serviceScope.ServiceProvider;

            try
            {
                var context = serviceProvider.GetRequiredService<LibraryDbContext>();
                await DbInitializer.InitializeAsync(context);
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex.Message, "An error occurred while migrating or initializing the database.");
            }
        }
        
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