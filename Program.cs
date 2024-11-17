using static LibraryAPI.Configuration.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

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

