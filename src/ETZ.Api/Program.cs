using System.Text.Json.Serialization;
using ETZ.Persistence.Context;
using ETZ.Persistence.ServiceRegistiration;
using Microsoft.EntityFrameworkCore;    

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        // Enum'ları string olarak döndür (TR/EN gibi)
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (geliştirme için açık; ihtiyaç oldukça daralt)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Default", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DbContext
var connectionString = builder.Configuration.GetConnectionString("ETZ");
builder.Services.AddDbContext<ETZDbContext>(options =>
    options.UseNpgsql(connectionString));

// DI - Services
builder.Services.AddPersistenceServices();

var app = builder.Build();

// Apply pending EF Core migrations at startup (Render)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ETZDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Default");
app.UseAuthorization();

app.MapControllers();

app.Run();
