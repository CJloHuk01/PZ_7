using pz_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7.Interfaces
{
    public interface ICurrencyConverter
    {
        double Convert(CurrencyData data);
        bool IsSupportedCurrencyPair(string from, string to);
    }
}
