using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Domain.Entities;
using GestaoFinanceiraApi.Infrastructure.Connection;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceiraApi.Infrastructure.Repositories
{
    public class UsuarioRepository
    {
        private readonly AppDbContext _dbContext;

        public UsuarioRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Usuario?> BuscarUsuarioPorIdAsync(int id)
        {
            return await _dbContext.Usuarios
                .Include(u => u.Despesas)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> AdicionarUsuarioAsync(Usuario usuario)
        {  
                var result = await _dbContext.Usuarios.AddAsync(usuario);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
        }

        public async Task<Usuario?> AtualizarSaldoAsync(int id, AtualizarSaldoRequestDto dto)
        {
           
                var existingUsuario = await _dbContext.Usuarios.FindAsync(id);
                if (existingUsuario == null)
                {
                    return null;
                }
                existingUsuario.Saldo += dto.AtualizacaoSaldo;
                var result =  _dbContext.Update(existingUsuario);
                await _dbContext.SaveChangesAsync();
                
                return result.Entity;
        }
        public async Task<Usuario?> DeletarUsuarioAsync(int id)
        {
            var existingUsuario = await _dbContext.Usuarios.FindAsync(id);
            if (existingUsuario == null)
            {
                return null;
            }
            _dbContext.Usuarios.Remove(existingUsuario);
            await _dbContext.SaveChangesAsync();
            return existingUsuario;
        }

        public async Task<List<Usuario>> ListarTodosUsuariosAsync()
        {
            return await _dbContext.Usuarios
                .Include(u => u.Despesas)
                .ToListAsync();
        }
    }
}
