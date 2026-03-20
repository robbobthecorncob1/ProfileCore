using Microsoft.EntityFrameworkCore;
using ProfileCore.Data;
using ProfileCore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowDevEnvironments", policy => policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!).AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IGpaCalculatorService, GpaCalculatorService>();
builder.Services.AddDbContext<WebsiteDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowDevEnvironments");

app.UseAuthorization();

app.MapControllers();

app.Run();
