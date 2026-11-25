using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Domain.Entities;
using GestaoFinanceiraApi.Infrastructure.Repositories;
using GestaoFinanceiraApi.Shared;

namespace GestaoFinanceiraApi.Application.Services
{
    public class DespesaService
    {
        public readonly DespesaRepository _despesaRepository;
        public readonly UsuarioRepository _usuarioRepository;

        public DespesaService(DespesaRepository despesaRepository, UsuarioRepository usuarioRepository)
        {
            _despesaRepository = despesaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<CriarDespesaResponseDto?> AdicionarDespesaAsync(CriarDespesaRequestDto dto)
        {
            if (dto.Valor <= 0)
                throw new ArgumentException("O valor da despesa deve ser maior que zero.");

            var user =  await _usuarioRepository.BuscarUsuarioPorIdAsync(dto.UsuarioId)
                ?? throw new ArgumentException($"O usuário não foi encontrado pelo busca por ID: '{dto.UsuarioId}'.");       

            var entity = new Despesa
            {
                UsuarioId = dto.UsuarioId,
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                DataCriacao = DateTimeHelper.ConvertToBrazilianFormat(DateTime.Now)
            };

            await _despesaRepository.AdicionarDespesaAsync(entity);

            return new CriarDespesaResponseDto(user.Nome, entity.Descricao, entity.Valor, entity.UsuarioId, entity.DataCriacao);
        }

    }
}
