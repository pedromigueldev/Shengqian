#!/usr/bin/env -S dotnet --

#:sdk Microsoft.NET.Sdk.Web
#:property TargetFramework=net11.0
#:property PublishAot=false

#:package Microsoft.AspNetCore.OpenApi@11.0.0
#:package Microsoft.EntityFrameworkCore.Design@11.0.0
#:package Microsoft.EntityFrameworkCore.Sqlite@11.0.0

#:include Vendor/SmartCtor/SmartCtor.cs
#:include src/**/*.cs

using Microsoft.EntityFrameworkCore;
using Shengqian.AppDbContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidProgramException("DefaultConnection is not present...");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlite(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();