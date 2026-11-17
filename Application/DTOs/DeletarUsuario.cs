namespace GestaoFinanceiraApi.Application.DTOs
{
    public record DeletarUsuarioRequestDto(int Id);

    public record DeletarUsuarioResponseDto(int Id, string Nome, string Email, decimal Saldo);
}
