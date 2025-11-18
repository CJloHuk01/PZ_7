using pz_7.Interfaces;
using pz_7.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var creditCalculator = new CreditCalculator();
            var currencyConverter = new CurrencyConverter();
            var depositCalculator = new DepositCalculator();

            var consoleInterface = new ConsoleInterface(
                creditCalculator,
                currencyConverter,
                depositCalculator
            );
            consoleInterface.Run();
        }
    }
}
