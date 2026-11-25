using GestaoFinanceiraApi.Domain.Entities;

using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestaoFinanceiraApi.Application.DTOs

{
    public record BuscarUsuarioRequestDto(int Id);
    public record BuscarUsuarioResponseDto(int Id, string Nome, string Email, decimal Saldo, IEnumerable<DespesaDetalheResponseDto> Despesas, decimal SaldoAtualizado);

    public record DespesaDetalheResponseDto(int Id, string Descricao, decimal Valor, DateTime DataCriacao, int UsuarioId);
}