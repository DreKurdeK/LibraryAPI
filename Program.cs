using Serilog;
using static LibraryAPI.Configuration.LoggerConfigurator;
using static LibraryAPI.Configuration.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogger();
builder.Host.UseSerilog();
await ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/home/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

