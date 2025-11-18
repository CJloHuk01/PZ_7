using pz_7.Interfaces;
using pz_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7.Services
{
    public class CreditCalculator : ICreditCalculator
    {
        public (double MonthlyPayment, double TotalAmount, double Overpayment) Calculate(CreditData data)
        {
            ValidateCreditData(data);

            double monthlyRate = data.InterestRate / 100 / 12;
            double monthlyPayment = data.Amount * (monthlyRate * Math.Pow(1 + monthlyRate, data.Months)) /
                                  (Math.Pow(1 + monthlyRate, data.Months) - 1);
            double totalAmount = monthlyPayment * data.Months;
            double overpayment = totalAmount - data.Amount;

            return (monthlyPayment, totalAmount, overpayment);
        }

        public List<PaymentSchedule> GetPaymentSchedule(CreditData data)
        {
            var schedule = new List<PaymentSchedule>();
            var calculation = Calculate(data);

            double remainingDebt = data.Amount;
            double monthlyRate = data.InterestRate / 100 / 12;

            for (int month = 1; month <= data.Months; month++)
            {
                double interestPart = remainingDebt * monthlyRate;
                double principalPart = calculation.MonthlyPayment - interestPart;
                remainingDebt -= principalPart;

                schedule.Add(new PaymentSchedule(month, principalPart, interestPart, Math.Max(remainingDebt, 0)));
            }

            return schedule;
        }

        private void ValidateCreditData(CreditData data)
        {
            if (data.Amount <= 0) throw new ArgumentException("Сумма кредита должна быть положительной");
            if (data.Months <= 0) throw new ArgumentException("Срок кредита должен быть положительным");
            if (data.InterestRate <= 0) throw new ArgumentException("Процентная ставка должна быть положительной");
        }
    }
}
