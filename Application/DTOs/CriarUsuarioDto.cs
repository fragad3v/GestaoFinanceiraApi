namespace GestaoFinanceiraApi.Application.DTOs
{
    public record CriarUsuarioRequestDto(string Nome, string Email);

    public record CriarUsuarioResponseDto(int Id, string Nome, string Email, decimal Saldo);
}
