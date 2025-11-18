using pz_7.Interfaces;
using pz_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz_7.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly Dictionary<string, double> _exchangeRates;

        public CurrencyConverter()
        {
            _exchangeRates = new Dictionary<string, double>
        {
            {"USD_RUB", 81.13},
            {"EUR_RUB", 95.1},
            {"EUR_USD", 1.16},
            {"RUB_USD", 1.0 / 81.13},
            {"RUB_EUR", 1.0 / 95.1},
            {"USD_EUR", 1.0 / 1.16}
        };
        }

        public double Convert(CurrencyData data)
        {
            ValidateCurrencyData(data);

            if (data.FromCurrency == data.ToCurrency)
                return data.Amount;

            string exchangeKey = $"{data.FromCurrency}_{data.ToCurrency}";
            if (!_exchangeRates.ContainsKey(exchangeKey))
                throw new ArgumentException($"Неподдерживаемая валютная пара: {data.FromCurrency}->{data.ToCurrency}");

            return data.Amount * _exchangeRates[exchangeKey];
        }

        public bool IsSupportedCurrencyPair(string from, string to)
        {
            if (from == to) return true;
            string exchangeKey = $"{from}_{to}";
            return _exchangeRates.ContainsKey(exchangeKey);
        }

        private void ValidateCurrencyData(CurrencyData data)
        {
            if (data.Amount <= 0) throw new ArgumentException("Сумма должна быть положительной");
            if (string.IsNullOrEmpty(data.FromCurrency)) throw new ArgumentException("Исходная валюта не указана");
            if (string.IsNullOrEmpty(data.ToCurrency)) throw new ArgumentException("Целевая валюта не указана");
        }
    }
}
