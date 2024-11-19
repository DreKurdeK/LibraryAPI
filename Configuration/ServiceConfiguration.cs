using System.Text.Json;
using FluentValidation;
using LibraryAPI.Data;
using LibraryAPI.Helpers;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using LibraryAPI.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LibraryAPI.Configuration;

public static class ServiceConfiguration
{
    public static async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        
        // DbContext Configuration
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
        
        
        // AutoMapper
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        
        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<BookDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthorDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<PublisherDtoValidator>();

        // Services
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IPublisherService, PublisherService>();

        // Repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        
        // Controllers and JSON Serialization
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Library API",
                Description = "API to manage books, authors and publishers.",
            });
        });
    }

}