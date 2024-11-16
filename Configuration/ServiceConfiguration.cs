using System.Diagnostics.Eventing.Reader;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryAPI.Validators;

namespace LibraryAPI.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<BookValidator>();
    }
}