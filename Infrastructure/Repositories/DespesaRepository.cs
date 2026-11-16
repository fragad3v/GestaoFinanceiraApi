using GestaoFinanceiraApi.Domain.Entities;
using GestaoFinanceiraApi.Infrastructure.Connection;

namespace GestaoFinanceiraApi.Infrastructure.Repositories
{
    public class DespesaRepository
    {
        private readonly AppDbContext _dbContext;


        public DespesaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AdicionarDespesa(Despesa despesa)
        {
            try
            {
                _dbContext.Despesas.Add(despesa);
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
