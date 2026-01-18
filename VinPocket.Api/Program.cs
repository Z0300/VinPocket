using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Scalar.AspNetCore;
using VinPocket.Api.Data;
using VinPocket.Api.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), npgSqlOptions => 
        npgSqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName))
    .UseSnakeCaseNamingConvention());

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<TokenProvider>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/openapi/{documentName}.json");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
