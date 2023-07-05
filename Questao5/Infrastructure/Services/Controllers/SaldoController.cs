using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : ControllerBase
    {
        private readonly IDbContext _dbContext;

        public SaldoController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{idContaCorrente}")]
        public async Task<IActionResult> GetSaldoContaCorrente(string idContaCorrente)
        {
            try
            {
                // Verificar se a conta corrente existe e está ativa
                var contaCorrente = await _dbContext.GetContaCorrenteByIdAsync(idContaCorrente);
                if (contaCorrente == null)
                {
                    return NotFound(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta corrente não encontrada." });
                }

                if (!contaCorrente.Ativo)
                {
                    return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Conta corrente inativa." });
                }

                // Calcular o saldo da conta corrente
                var saldo = await _dbContext.CalcularSaldoContaCorrenteAsync(idContaCorrente);

                // Construir a resposta com os dados do saldo
                var response = new SaldoContaResponse
                {
                    NumeroContaCorrente = contaCorrente.Numero,
                    NomeTitular = contaCorrente.Nome,
                    DataHoraConsulta = DateTime.Now,
                    SaldoAtual = saldo
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Tipo = "INTERNAL_ERROR", Mensagem = "Erro interno no servidor." });
            }
        }
    }
}

