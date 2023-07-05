using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Sqlite.Models;

namespace Questao5.Infrastructure.Sqlite
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;

        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ContaCorrente>> GetContasCorrentesAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM contacorrente";
                return await connection.QueryAsync<ContaCorrente>(query);
            }
        }

        public async Task<ContaCorrente> GetContaCorrenteByIdAsync(string idContaCorrente)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente";
                return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { IdContaCorrente = idContaCorrente });
            }
        }


        public async Task<string> CriarMovimentoAsync(Movimento request)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var movimentoId = Guid.NewGuid().ToString();
                var query = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                            "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
                var parameters = new
                {
                    IdMovimento = movimentoId,
                    IdContaCorrente = request.IdContaCorrente,
                    DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                    TipoMovimento = request.TipoMovimento,
                    Valor = request.Valor
                };
                await connection.ExecuteAsync(query, parameters);
                return movimentoId;
            }
        }

        public async Task<decimal> CalcularSaldoContaCorrenteAsync(string idContaCorrente)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var query = "SELECT SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END) " +
                            "FROM movimento WHERE idcontacorrente = @IdContaCorrente";
                var parameters = new { IdContaCorrente = idContaCorrente };
                var saldo = await connection.ExecuteScalarAsync<decimal>(query, parameters);
                return saldo;
            }
        }
    }
}
