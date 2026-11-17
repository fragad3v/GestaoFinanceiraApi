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

        public Usuario? GetUsuarioById(int id)
        {
            return _dbContext.Usuarios
                .Include(u => u.Despesas)
                .FirstOrDefault(u => u.Id == id);

        }

        public Usuario? AdicionarUsuario(Usuario usuario)
        {
            try
            {
                var result = _dbContext.Usuarios.Add(usuario);
                _dbContext.SaveChanges();
                return result.Entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Usuario? AtualizarSaldo(int id, AtualizarSaldoRequestDto dto)
        {
           
                var existingUsuario = _dbContext.Usuarios.Find(id);
                if (existingUsuario == null)
                {
                    return null;
                }
                existingUsuario.Saldo += dto.AtualizacaoSaldo;
                var result = _dbContext.Update(existingUsuario);
                _dbContext.SaveChanges();
                
                return result.Entity;
        }
    }
}
