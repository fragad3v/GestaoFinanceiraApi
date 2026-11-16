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

        public CriarDespesaResponseDto? AdicionarDespesa(CriarDespesaRequestDto despesa)
        {
            var entity = new Despesa
            {
                UsuarioId = despesa.UsuarioId,
                Descricao = despesa.Descricao,
                Valor = despesa.Valor
            };
            var sucesso = _despesaRepository.AdicionarDespesa(entity);

            if (!sucesso)
            {
                return null;
            }
            return new CriarDespesaResponseDto(entity.Descricao, entity.Valor, entity.UsuarioId.Value);
        }
    }
}
