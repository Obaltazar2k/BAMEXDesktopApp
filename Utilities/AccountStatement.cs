using System;

namespace BAMEX.Utilities
{
    class AccountStatement
    {
        public string Concepto { get; set; }
        public DateTime? Fecha { get; set; }
        public double? Monto { get; set; }
        public string TipoDeMovimiento { get; set; }
    }
}
