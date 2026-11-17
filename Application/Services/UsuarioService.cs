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

        public Usuario GetUsuarioById(int id)
        {
            return _usuarioRepository.GetUsuarioById(id);
        }

        public CriarUsuarioResponseDto? CriarUsuario(CriarUsuarioRequestDto dto)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Saldo = 0 // Inicializa o saldo como 0
            };
            // Aqui você pode adicionar a lógica para salvar o usuário no banco de dados
            // Por exemplo, usando um repositório ou contexto de banco de dados
            var usuarioCriado = _usuarioRepository.AdicionarUsuario(usuario);

            if (usuarioCriado == null)
            {
                return null;
            }

            return new CriarUsuarioResponseDto(usuarioCriado.Nome, usuarioCriado.Email, usuarioCriado.Saldo);
        }

        public AtualizarSaldoResponseDto? AtualizarSaldo(int id, AtualizarSaldoRequestDto dto)
        {

            var usuarioAtualizado = _usuarioRepository.AtualizarSaldo(id, dto);
            if (usuarioAtualizado == null)
            {
                return null;
            }
            return new AtualizarSaldoResponseDto(usuarioAtualizado.Saldo);
        }
    }
}
