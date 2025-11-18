using pz_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7.Interfaces
{
    public interface IDepositCalculator
    {
        (double Income, double TotalAmount) Calculate(DepositData data);
    }
}
