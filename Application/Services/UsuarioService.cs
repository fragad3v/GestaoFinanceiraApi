using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Domain.Entities;
using GestaoFinanceiraApi.Infrastructure.Repositories;

namespace GestaoFinanceiraApi.Application.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<BuscarUsuarioResponseDto?> BuscarUsuarioPorIdAsync(int id)
        {
            var u =  await _usuarioRepository.BuscarUsuarioPorIdAsync(id);
            if (u is null) return null;

            return new BuscarUsuarioResponseDto(
               u.Id,
               u.Nome,
               u.Email,
               u.Saldo,
               u.Despesas
                   .Select(d => new DespesaDetalheResponseDto(d.Id, d.Descricao, d.Valor, d.DataCriacao, d.UsuarioId))
                   .ToList(),
               u.Saldo - u.Despesas.Sum(d => d.Valor));
        }

        public async Task<CriarUsuarioResponseDto?> CriarUsuarioAsync(CriarUsuarioRequestDto dto)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Saldo = 0 
            };
            
            var usuarioCriado = await _usuarioRepository.AdicionarUsuarioAsync(usuario);

            if (usuarioCriado == null)
            {
                return null;
            }

            return new CriarUsuarioResponseDto(usuarioCriado.Nome, usuarioCriado.Email, usuarioCriado.Saldo);
        }

        public async Task<AtualizarSaldoResponseDto?> AtualizarSaldoAsync(int id, AtualizarSaldoRequestDto dto)
        {

            var usuarioAtualizado = await _usuarioRepository.AtualizarSaldoAsync(id, dto);
            if (usuarioAtualizado == null)
            {
                return null;
            }
            return new AtualizarSaldoResponseDto(usuarioAtualizado.Saldo);
        }
        public async Task<Usuario?> DeletarUsuarioAsync(int id)
        {
            return await _usuarioRepository.DeletarUsuarioAsync(id);
        }

        public async Task<List<ListarUsuarioResponseDto>> ListarTodosUsuariosAsync()
        {
            var entities = await _usuarioRepository.ListarTodosUsuariosAsync();

            return entities.ConvertAll(u => new ListarUsuarioResponseDto
            (
                u.Id,
                u.Nome,
                u.Email,
                u.Saldo,
                u.Despesas
                    .Select(d => new DespesaResponseDto(d.Descricao, d.Valor, d.DataCriacao))
                    .ToList(),
                u.Saldo - u.Despesas.Sum(d => d.Valor)
            ));
        }
    }
}
