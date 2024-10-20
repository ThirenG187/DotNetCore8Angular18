using Microsoft.EntityFrameworkCore;
using Skinet.Infrastructure.Data;
using Skinet.Infrastructure.Keys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(ConfigurationSettingsKey.DatabaseConnection));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
