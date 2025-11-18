using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7.Models
{
    public class DepositData
    {
        public double Amount { get; set; }
        public int Months { get; set; }
        public double InterestRate { get; set; }
        public bool WithCapitalization { get; set; }
    }
}
