using System.Text.Json;
using System.Text.Json.Serialization;
using GestaoFinanceiraApi;
using Microsoft.EntityFrameworkCore;

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

app.MapPost("/usuarios", async (CriarUsuarioDto dto, AppDbContext db) =>
{
    var usuario = new Usuario { Nome = dto.Nome, Email = dto.Email };
    db.Usuarios.Add(usuario);
    await db.SaveChangesAsync()

    return Results.Created($"/usuarios/{usuario.Id}", usuario);
})
.Produces<Usuario>(StatusCodes.Status201Created); ; // Melhora a integração com a documentação OpenAPI
app.MapPost("/usuarios/saldo", async (int id, AtualizarSaldoDto dto, AppDbContext db) =>
{
    var usuario = await db.Usuarios
         .Include(u => u.Despesas)
         .FirstOrDefaultAsync(u => u.Id == id);
    usuario.Saldo += dto.NovoSaldo;
    db.Usuarios.Update(usuario);
    await db.SaveChangesAsync();

    return Results.Created($"/usuarios/{usuario.Id}", usuario);
})
.Produces<Usuario>(StatusCodes.Status201Created); ; // Melhora a integração com a documentação OpenAPI

app.MapGet("/usuarios/{id:int}", async (int id, AppDbContext db) =>
{
    var usuario = await db.Usuarios
        .Include(u => u.Despesas)
        .FirstOrDefaultAsync(u => u.Id == id);

    if (usuario is null) return Results.NotFound(new { mensagem = "Usuário não encontrado." });

    return Results.Ok(usuario);
})
.Produces<Usuario>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// (Demais endpoints de despesas continuam aqui...)

app.Run();