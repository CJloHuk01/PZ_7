using pz_7.Models;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace pz_7.Interfaces
{
    public class ConsoleInterface
    {
        private readonly ICreditCalculator _creditCalculator;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IDepositCalculator _depositCalculator;
        private readonly bool _skipClear;
        private readonly bool _skipWait;

        public ConsoleInterface(ICreditCalculator creditCalculator,
                              ICurrencyConverter currencyConverter,
                              IDepositCalculator depositCalculator,
                              bool skipClear = false,
                              bool skipWait = false)
        {
            _creditCalculator = creditCalculator;
            _currencyConverter = currencyConverter;
            _depositCalculator = depositCalculator;
            _skipClear = skipClear;
            _skipWait = skipWait;
        }

        public void Run()
        {
            while (true)
            {
                if (!_skipClear) Console.Clear();

                ShowMainMenu();

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        HandleCreditCalculation();
                        break;
                    case "2":
                        HandleCurrencyConversion();
                        break;
                    case "3":
                        HandleDepositCalculation();
                        break;
                    case "4":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор! Нажмите любую клавишу...");
                        if (!_skipWait) Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("1. Расчет кредита");
            Console.WriteLine("2. Конвертер валют");
            Console.WriteLine("3. Калькулятор вкладов");
            Console.WriteLine("4. Выход");
            Console.WriteLine("");
            Console.Write("Выберите опцию: ");
        }

        private void HandleCreditCalculation()
        {
            if (!_skipClear) Console.Clear();

            try
            {
                var data = new CreditData
                {
                    Amount = GetPositiveDoubleInput("Сумма кредита (руб): "),
                    Months = GetPositiveIntInput("Срок кредита (месяцев): "),
                    InterestRate = GetPositiveDoubleInput("Процентная ставка (% годовых): ")
                };

                var result = _creditCalculator.Calculate(data);
                var schedule = _creditCalculator.GetPaymentSchedule(data);

                DisplayCreditResults(result, schedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        private void HandleCurrencyConversion()
        {
            if (!_skipClear) Console.Clear();

            try
            {
                var data = new CurrencyData
                {
                    FromCurrency = GetCurrencyInput("Исходная валюта (RUB, USD, EUR): "),
                    ToCurrency = GetCurrencyInput("Целевая валюта (RUB, USD, EUR): "),
                    Amount = GetPositiveDoubleInput("Сумма для конвертации: ")
                };

                double result = _currencyConverter.Convert(data);
                Console.WriteLine($"\nРезультат конвертации: {result:F2} {data.ToCurrency}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        private void HandleDepositCalculation()
        {
            if (!_skipClear) Console.Clear();

            try
            {
                var data = new DepositData
                {
                    Amount = GetPositiveDoubleInput("Сумма вклада (руб): "),
                    Months = GetPositiveIntInput("Срок вклада (месяцев): "),
                    InterestRate = GetPositiveDoubleInput("Процентная ставка (% годовых): "),
                    WithCapitalization = GetDepositTypeInput()
                };

                var result = _depositCalculator.Calculate(data);
                DisplayDepositResults(data, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        private double GetPositiveDoubleInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double result) && result > 0)
                    return result;
                Console.WriteLine("Ошибка! Введите положительное число.");
            }
        }

        private int GetPositiveIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result) && result > 0)
                    return result;
                Console.WriteLine("Ошибка! Введите целое положительное число.");
            }
        }

        private string GetCurrencyInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.ToUpper();
                if (input == "RUB" || input == "USD" || input == "EUR")
                    return input;
                Console.WriteLine("Ошибка! Допустимые валюты: RUB, USD, EUR.");
            }
        }

        private bool GetDepositTypeInput()
        {
            while (true)
            {
                Console.Write("Тип вклада (1 - с капитализацией, 2 - без капитализации): ");
                string input = Console.ReadLine();
                if (input == "1") return true;
                if (input == "2") return false;
                Console.WriteLine("Ошибка! Введите 1 или 2.");
            }
        }

        private void DisplayCreditResults((double MonthlyPayment, double TotalAmount, double Overpayment) result,
                                        List<PaymentSchedule> schedule)
        {
            Console.WriteLine("\nРЕЗУЛЬТАТЫ РАСЧЕТА:");
            Console.WriteLine($"Ежемесячный платеж: {result.MonthlyPayment:F2} руб");
            Console.WriteLine($"Общая сумма выплат: {result.TotalAmount:F2} руб");
            Console.WriteLine($"Переплата по кредиту: {result.Overpayment:F2} руб");
        }

        private void DisplayDepositResults(DepositData data, (double Income, double TotalAmount) result)
        {
            Console.WriteLine("\nРЕЗУЛЬТАТЫ РАСЧЕТА:");
            Console.WriteLine($"Тип вклада: {(data.WithCapitalization ? "с капитализацией" : "без капитализации")}");
            Console.WriteLine($"Сумма вклада: {data.Amount:F2} руб");
            Console.WriteLine($"Доход по вкладу: {result.Income:F2} руб");
            Console.WriteLine($"Итоговая сумма: {result.TotalAmount:F2} руб");
        }

        private void WaitForContinue()
        {
            if (!_skipWait)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}
