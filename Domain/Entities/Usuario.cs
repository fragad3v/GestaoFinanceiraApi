using System.Text.Json.Serialization;

namespace GestaoFinanceiraApi.Domain.Entities
{
    public class Usuario
    {
        private decimal _saldo;

        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
   
        public decimal Saldo
        {
            get => _saldo;
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("O saldo não pode ser negativo!");
                }

                _saldo = value;
            }
        }

        
        public List<Despesa> Despesas { get; set; } = new();

    }
}
