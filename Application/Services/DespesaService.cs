using GestaoFinanceiraApi.Application.DTOs;
using GestaoFinanceiraApi.Domain.Entities;
using GestaoFinanceiraApi.Infrastructure.Repositories;

namespace GestaoFinanceiraApi.Application.Services
{
    public class DespesaService
    {
        public readonly DespesaRepository _despesaRepository;

        public DespesaService(DespesaRepository despesaRepository)
        {
            _despesaRepository = despesaRepository;
        }

        public async Task<CriarDespesaResponseDto?> AdicionarDespesaAsync(CriarDespesaRequestDto dto)
        {
            if (dto.Valor <= 0)
                throw new ArgumentException("O valor da despesa deve ser maior que zero.");

            if (!await _despesaRepository.UsuarioExisteAsync(dto.UsuarioId))
                return null;

            var entity = new Despesa
            {
                UsuarioId = dto.UsuarioId,
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                DataCriacao = DateTime.UtcNow
            };

            await _despesaRepository.AdicionarDespesaAsync(entity);

            return new CriarDespesaResponseDto(entity.Descricao, entity.Valor, entity.UsuarioId);
        }
    }
}
