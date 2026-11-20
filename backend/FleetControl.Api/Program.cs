
using FleetControl.Api.Infrastructure.Data;
using FleetControl.Api.ExceptionHandlers;
using Microsoft.EntityFrameworkCore;
using FleetControl.Api.Application.Interfaces;
using FleetControl.Api.Application.Services;

var builder = WebApplication.CreateBuilder(args);
// Controllers
builder.Services.AddControllers();

// Application Services
builder.Services.AddScoped<IEquipamentoService, EquipamentoService>();

// Tratamento global de exceções
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// PostgreSQL e PostGIS
builder.Services.AddDbContext<FleetControlDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
    );
});

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Tratamento global de exceções
app.UseExceptionHandler();

// Teste de conexão com o banco
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<FleetControlDbContext>();

    Console.WriteLine($"Banco conectado: {db.Database.CanConnect()}");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
