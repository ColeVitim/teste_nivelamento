﻿namespace Questao5.Infrastructure.Sqlite.Models
{

    public class Movimento
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }

}
