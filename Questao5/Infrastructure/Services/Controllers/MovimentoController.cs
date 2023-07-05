using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Sqlite.Models;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly IDbContext _dbContext;

        public MovimentoController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentoContaRequest request)
        {
            try
            {
                // Verificar se a conta corrente está cadastrada
                var contaCorrente = await _dbContext.GetContaCorrenteByIdAsync(request.IdContaCorrente);
                if (contaCorrente == null)
                {
                    return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "A conta corrente não está cadastrada." });
                }

                // Verificar se a conta corrente está ativa
                if (!contaCorrente.Ativo)
                {
                    return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "A conta corrente está inativa." });
                }

                // Verificar se o valor é positivo
                if (request.Valor <= 0)
                {
                    return BadRequest(new { Tipo = "INVALID_VALUE", Mensagem = "O valor deve ser positivo." });
                }

                // Verificar se o tipo de movimento é válido
                if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                {
                    return BadRequest(new { Tipo = "INVALID_TYPE", Mensagem = "O tipo de movimento deve ser 'C' (crédito) ou 'D' (débito)." });
                }

                // Gerar ID do movimento
                var idMovimento = Guid.NewGuid().ToString();

                // Inserir movimento no banco de dados
                var movimento = new Movimento
                {
                    IdMovimento = idMovimento,
                    IdContaCorrente = request.IdContaCorrente,
                    DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                    TipoMovimento = request.TipoMovimento,
                    Valor = request.Valor
                };

                await _dbContext.CriarMovimentoAsync(movimento);
               

                return Ok(new { IdMovimento = idMovimento });
            }
            catch (Exception ex)
            {
                // Tratar erros inesperados e retornar uma resposta adequada
                return StatusCode(500, new { Tipo = "INTERNAL_SERVER_ERROR", Mensagem = "Ocorreu um erro interno no servidor." });
            }
        }
    }
}