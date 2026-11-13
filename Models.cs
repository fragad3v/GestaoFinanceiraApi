namespace GestaoFinanceiraApi;

using System.Text.Json.Serialization;
public class Models
{

}

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Saldo { get; set; }

    [JsonIgnore]
    public List<Despesa> Despesas { get; set; } = new();

}

public class Despesa
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataCriacao { get; set; }

    public int UsuarioId { get; set; }

    [JsonIgnore]
    public Usuario? Usuario { get; set; }

    public bool SubtrairValor(decimal quantia)
    {
        if (quantia <= 0)
        {
            throw new ArgumentException("A quantia a ser subtraída deve ser maior que zero.");
        }
        if (quantia > Valor)
        {
            if (Usuario != null)
            {
                Usuario.Saldo += Valor; // Adiciona o valor restante ao saldo do usuário
            }
        }
        Valor -= quantia;
        return Valor == 0; // Subtração bem-sucedida
    }

}

public record CriarUsuarioDto(string Nome, string Email);
public record CriarDespesaDto(string Descricao, decimal Valor);
public record SubtrairDespesasDto(decimal ValorSubtrair);
public record AtualizarSaldoDto(decimal NovoSaldo);
