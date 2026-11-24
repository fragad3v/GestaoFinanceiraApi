using GestaoFinanceiraApi.Domain.Entities;

using System.Text.Json.Serialization;

namespace GestaoFinanceiraApi.Application.DTOs

{
    public record BuscarUsuarioRequestDto(int Id);
    public record BuscarUsuarioResponseDto(string Nome, string Email, decimal Saldo, IEnumerable<Despesa> Depesas, decimal SaldoAtualizado);
    

}
