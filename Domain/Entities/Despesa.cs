using System.Text.Json.Serialization;

namespace GestaoFinanceiraApi.Domain.Entities
{
    public class Despesa
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }

        public int UsuarioId { get; set; }

        [JsonIgnore]
        public required Usuario? Usuario { get; set; }

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
                    Usuario.Saldo += Valor; 
                }
            }
            Valor -= quantia;
            return Valor == 0; 
        }

    }
}
