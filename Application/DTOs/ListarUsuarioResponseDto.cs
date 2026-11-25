namespace GestaoFinanceiraApi.Application.DTOs
{
    public record DespesaResponseDto(string Descricao, decimal Valor, DateTime DataCriacao);
    public record ListarUsuarioResponseDto(
        int Id,
        string Nome,
        string Email,
        decimal Saldo,
        IEnumerable<DespesaResponseDto> Despesas,
        decimal SaldoAtualizado);
}
