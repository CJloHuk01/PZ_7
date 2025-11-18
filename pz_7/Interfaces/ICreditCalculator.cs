using pz_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7.Interfaces
{
    public class PaymentSchedule
    {
        public int Month { get; }
        public double Principal { get; }
        public double Interest { get; }
        public double RemainingDebt { get; }

        public PaymentSchedule(int month, double principal, double interest, double remainingDebt)
        {
            Month = month;
            Principal = principal;
            Interest = interest;
            RemainingDebt = remainingDebt;
        }
    }

    public interface ICreditCalculator
    {
        (double MonthlyPayment, double TotalAmount, double Overpayment) Calculate(CreditData data);
        List<PaymentSchedule> GetPaymentSchedule(CreditData data);
    }

}
