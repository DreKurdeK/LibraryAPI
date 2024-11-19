using Serilog;
using static LibraryAPI.Configuration.LoggerConfigurator;
using static LibraryAPI.Configuration.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogger();
builder.Host.UseSerilog();
await ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler("/error");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

