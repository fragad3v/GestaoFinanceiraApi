namespace GestaoFinanceiraApi.Application.DTOs
{
    public record SubtrairDespesasRequestDto(decimal ValorSubtrair);

    public record SubtrairDespesasResponseDto(decimal ValorRestante);

}
