using System.Text.Json.Serialization;

namespace GestaoFinanceiraApi.Domain.Entities
{
    public class Despesa
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string DataCriacao { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; } = null;
    }
}
