using GestaoFinanceiraApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceiraApi.Infrastructure.Connection;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Despesa> Despesas => Set<Despesa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Despesa>()
            .Property(d => d.Valor)
            .HasPrecision(18, 2); // Boa prática para valores monetários
    }
}