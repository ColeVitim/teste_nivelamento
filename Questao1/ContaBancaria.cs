using System.Globalization;

namespace Questao1
{
    class ContaBancaria {
        public  long _numeroConta { get; set; }
        public string _nomeTitular { get; set; }
        public double _saldo { get; set; }
        public string mensagem { get; set; }
        public ContaBancaria(long numeroConta, string nomeTitular, double saldo = 0)
        {
            _numeroConta = numeroConta;
            _nomeTitular = nomeTitular;
            _saldo = saldo;
            mensagem = $"Dados da conta \n Conta {_numeroConta}, Titular: {_nomeTitular}, Saldo: $ {_saldo}";
        }

        public void Deposito(double quantia)
        {
            _saldo += quantia;
            mensagem = $"Dados da conta \n Conta {_numeroConta}, Titular: {_nomeTitular}, Saldo: $ {_saldo}";
        }
        public void Saque(double quantia)
        {
            if (_saldo >= quantia)
            {
                _saldo -= quantia;
                mensagem = $"Dados da conta \n Conta {_numeroConta}, Titular: {_nomeTitular}, Saldo: $ {_saldo}";
            }
            else
            {
                mensagem = $"Dados da conta \n Quantia maior que o salto existente \n Conta {_numeroConta}, Titular: {_nomeTitular}, Saldo: $ {_saldo}";
            }
            
        }
    }
}
