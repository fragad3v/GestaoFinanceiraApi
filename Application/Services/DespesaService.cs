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

        public async Task<CriarDespesaResponseDto?> AdicionarDespesaAsync(CriarDespesaRequestDto despesa)
        {
            var entity = new Despesa
            {
                Usuario = null,
                UsuarioId = despesa.UsuarioId,
                Descricao = despesa.Descricao,
                Valor = despesa.Valor
            };
            var sucesso = await _despesaRepository.AdicionarDespesaAsync(entity);

            if (!sucesso)
            {
                return null;
            }
            return new CriarDespesaResponseDto(entity.Descricao, entity.Valor, entity.UsuarioId);
        }

         
    }
}
