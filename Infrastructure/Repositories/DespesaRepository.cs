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

        public async Task<bool> AdicionarDespesaAsync(Despesa despesa)
        {

            await _dbContext.Despesas.AddAsync(despesa);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}

