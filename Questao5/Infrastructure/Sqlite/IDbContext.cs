using System.Collections.Generic;
using System.Threading.Tasks;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Sqlite.Models;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDbContext
    {
        Task<ContaCorrente> GetContaCorrenteByIdAsync(string idContaCorrente);
        Task<string> CriarMovimentoAsync(Movimento request);
        Task<decimal> CalcularSaldoContaCorrenteAsync(string idContaCorrente);

    }
}
