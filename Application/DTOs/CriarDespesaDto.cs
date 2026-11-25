namespace GestaoFinanceiraApi.Application.DTOs
{
    public record CriarDespesaRequestDto(string Descricao, decimal Valor, int UsuarioId);

    public record CriarDespesaResponseDto(string NomeUsuario, string Descricao, decimal Valor, int UsuarioId, string DataCriacao);
}
