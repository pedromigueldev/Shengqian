#!/usr/bin/env -S dotnet --

#:sdk Microsoft.NET.Sdk.Web
#:property TargetFramework=net11.0
#:property EmitCompilerGeneratedFiles=true
#:property PublishAot=false

#:package Microsoft.AspNetCore.OpenApi@11.0.0
#:package Microsoft.EntityFrameworkCore.Sqlite@11.0.0

#:include Vendor/SmartCtor/SmartCtor.cs
#:include Controllers/*.cs
#:include Domain/*.cs
#:include Dto/*.cs
#:include DbContext.cs

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Shengqian.Domain;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(c =>
{
    c.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=Shenqian.db");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    _ = db.Database.EnsureCreated();
}

app.MapGet("/api/hello", () => "hello");

app.Run();

[JsonSerializable(typeof(Transaction))]
[JsonSerializable(typeof(Category))]
internal partial class AppJsonSerializerContext : JsonSerializerContext {}