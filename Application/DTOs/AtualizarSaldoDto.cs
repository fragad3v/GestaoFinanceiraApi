namespace GestaoFinanceiraApi.Application.DTOs
{
    public record AtualizarSaldoRequestDto(decimal AtualizacaoSaldo);

    public record AtualizarSaldoResponseDto(decimal NovoSaldo);


}
