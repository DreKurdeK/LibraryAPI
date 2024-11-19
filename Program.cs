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
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

