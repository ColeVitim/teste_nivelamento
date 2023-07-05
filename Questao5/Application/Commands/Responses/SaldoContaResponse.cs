namespace Questao5.Application.Commands.Responses
{
    public class SaldoContaResponse
    {
        public int NumeroContaCorrente { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
