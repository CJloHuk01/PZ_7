using pz_7.Interfaces;
using pz_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7.Services
{
    public class DepositCalculator : IDepositCalculator
    {
        public (double Income, double TotalAmount) Calculate(DepositData data)
        {
            ValidateDepositData(data);

            if (data.WithCapitalization)
            {
                return CalculateWithCapitalization(data);
            }
            else
            {
                return CalculateWithoutCapitalization(data);
            }
        }

        private (double Income, double TotalAmount) CalculateWithCapitalization(DepositData data)
        {
            double totalAmount = data.Amount * Math.Pow(1 + data.InterestRate / 100 / 12, data.Months);
            double income = totalAmount - data.Amount;
            return (income, totalAmount);
        }

        private (double Income, double TotalAmount) CalculateWithoutCapitalization(DepositData data)
        {
            double income = data.Amount * data.InterestRate * data.Months / 12 / 100;
            double totalAmount = data.Amount + income;
            return (income, totalAmount);
        }

        private void ValidateDepositData(DepositData data)
        {
            if (data.Amount <= 0) throw new ArgumentException("Сумма вклада должна быть положительной");
            if (data.Months <= 0) throw new ArgumentException("Срок вклада должен быть положительным");
            if (data.InterestRate <= 0) throw new ArgumentException("Процентная ставка должна быть положительной");
        }
    }
}
