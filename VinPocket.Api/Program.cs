using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using VinPocket.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();
builder.AddDatabase();
builder.AddAuthenticationServices();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
