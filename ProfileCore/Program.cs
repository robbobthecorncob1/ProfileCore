using Microsoft.EntityFrameworkCore;
using ProfileCore.Data;
using ProfileCore.Endpoints;
using ProfileCore.Services;
using Resend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {options.AddPolicy("AllowDevEnvironments", policy => policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!).AllowAnyMethod().AllowAnyHeader());});
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IGpaCalculatorService, GpaCalculatorService>();
builder.Services.AddScoped<IWebsiteService, WebsiteService>();
builder.Services.AddDbContext<WebsiteDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(options => {options.ApiToken = builder.Configuration["Resend:ApiKey"]!;});
builder.Services.AddTransient<IResend, ResendClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();
app.UseCors("AllowDevEnvironments");
app.MapWebsiteEndpoints();
app.UseAuthorization();
app.MapControllers();
app.Run();
