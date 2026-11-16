using GestaoFinanceiraApi;
using GestaoFinanceiraApi.API;
using GestaoFinanceiraApi.Application.Services;
using GestaoFinanceiraApi.Infrastructure.Connection;
using GestaoFinanceiraApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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

// === ADICIONE ESTAS LINHAS PARA O SWAGGER ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ============================================

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


// === ADICIONE ESTAS LINHAS NO PIPELINE ===
// (Geralmente ativado em ambiente de desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Disponibiliza a interface visual em /swagger
}
// ========================================

// Middleware global de erros
app.UseMiddleware<ExceptionMiddleware>();

// ==========================================
// ENDPOINTS (Exemplos com documentação extra)
// ==========================================

var apiRest = new ApiRest(app);
apiRest.MapEndpoints();


// (Demais endpoints de despesas continuam aqui...)

app.Run();