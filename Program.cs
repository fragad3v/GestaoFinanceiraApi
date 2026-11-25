using GestaoFinanceiraApi.API;
using GestaoFinanceiraApi.API.Gateways;
using GestaoFinanceiraApi.Application.Services;
using GestaoFinanceiraApi.Infrastructure.Connection;
using GestaoFinanceiraApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=financas.db"));

// 2. Configuração do JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gestão Financeira API",
        Version = "v1",
        Description = "API REST para controle de usuários, saldo e despesas. Projeto de estudo em .NET 8 com Clean Architecture."
    });
});

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<DespesaRepository>();

// 3. Services
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<DespesaService>();

var app = builder.Build();

// Garante a criação do banco SQLite ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Disponibiliza a interface visual em /swagger
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

// Middleware global de erros
app.UseMiddleware<ExceptionMiddleware>();

var apiRest = new ApiRest();
apiRest.MapEndpoints(app);

app.Run();