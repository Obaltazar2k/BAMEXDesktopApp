using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMEX.Utilities
{
    class AccountInfoForClient
    {
        public string AccountNumber { get; set; }
        public double? Balance { get; set; }
        public string CardNumber { get; set; }
        public DateTime? CutDate { get; set; }
    }
}
