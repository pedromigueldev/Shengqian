using Microsoft.EntityFrameworkCore;
using Shengqian.AppDbContext;
using Shengqian.Backend.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new OptionalJsonConverterFactory());
    });
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