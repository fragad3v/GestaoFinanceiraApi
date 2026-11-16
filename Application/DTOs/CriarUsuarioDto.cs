namespace GestaoFinanceiraApi.Application.DTOs
{
    public record CriarUsuarioRequestDto(string Nome, string Email);

    public record CriarUsuarioResponseDto(string Nome, string Email, decimal Saldo);
}
