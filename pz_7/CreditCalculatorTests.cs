using Moq;
using pz_7.Interfaces;
using pz_7.Models;
using pz_7.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;

namespace pz_7
{
    public class CreditCalculatorTests
    {
        [Fact]
        public void Calculate_WithValidData_ReturnsCorrectResults()
        {
            var calculator = new CreditCalculator();
            var data = new CreditData { Amount = 100000, Months = 12, InterestRate = 10 };

            var result = calculator.Calculate(data);

            Assert.Equal(8791.59, result.MonthlyPayment, 2);
            Assert.Equal(105499.06, result.TotalAmount, 2);
            Assert.Equal(5499.06, result.Overpayment, 2);
        }

        [Theory]
        [InlineData(0, 12, 10, "Сумма кредита должна быть положительной")]
        [InlineData(100000, 0, 10, "Срок кредита должен быть положительным")]
        [InlineData(100000, 12, 0, "Процентная ставка должна быть положительной")]
        public void Calculate_WithInvalidData_ThrowsException(double amount, int months, double rate, string expectedMessage)
        {
            var calculator = new CreditCalculator();
            var data = new CreditData { Amount = amount, Months = months, InterestRate = rate };

            var exception = Assert.Throws<ArgumentException>(() => calculator.Calculate(data));
            Assert.Contains(expectedMessage, exception.Message);
        }
    }

    public class CurrencyConverterTests
    {
        [Theory]
        [InlineData(100, "USD", "RUB", 8113.0)]
        [InlineData(200, "EUR", "USD", 232.0)]
        [InlineData(10000, "RUB", "EUR", 105.15)]
        public void Convert_WithSupportedCurrencies_ReturnsCorrectResult(double amount, string from, string to, double expected)
        {
            var converter = new CurrencyConverter();
            var data = new CurrencyData { Amount = amount, FromCurrency = from, ToCurrency = to };

            var result = converter.Convert(data);

            Assert.Equal(expected, result, 2);
        }

        [Fact]
        public void Convert_WithUnsupportedCurrencyPair_ThrowsException()
        {
            var converter = new CurrencyConverter();
            var data = new CurrencyData { Amount = 100, FromCurrency = "USD", ToCurrency = "JPY" };

            Assert.Throws<ArgumentException>(() => converter.Convert(data));
        }
    }

    public class DepositCalculatorTests
    {
        [Fact]
        public void Calculate_WithoutCapitalization_ReturnsSimpleInterest()
        {
            var calculator = new DepositCalculator();
            var data = new DepositData
            {
                Amount = 100000,
                Months = 12,
                InterestRate = 8,
                WithCapitalization = false
            };

            var result = calculator.Calculate(data);

            Assert.Equal(8000.0, result.Income, 2);
            Assert.Equal(108000.0, result.TotalAmount, 2);
        }

        [Fact]
        public void Calculate_WithCapitalization_ReturnsCompoundInterest()
        {
            var calculator = new DepositCalculator();
            var data = new DepositData
            {
                Amount = 100000,
                Months = 12,
                InterestRate = 8,
                WithCapitalization = true
            };

            var result = calculator.Calculate(data);

            Assert.Equal(8300.0, result.Income, 1);
            Assert.Equal(108300.0, result.TotalAmount, 1);
        }
    }

    public class ConsoleInterfaceTests
    {
        [Fact]
        public void CreditCalculator_Calculate_Called_When_User_Selects_Credit_Option()
        {
            var mockCreditCalculator = new Mock<ICreditCalculator>();
            var mockCurrencyConverter = new Mock<ICurrencyConverter>();
            var mockDepositCalculator = new Mock<IDepositCalculator>();

            mockCreditCalculator.Setup(x => x.Calculate(It.IsAny<CreditData>()))
                .Returns((8791.59, 105499.08, 5499.08));

            var consoleInterface = new ConsoleInterface(
                mockCreditCalculator.Object,
                mockCurrencyConverter.Object,
                mockDepositCalculator.Object,
                skipClear: true,
                skipWait: true
            );

            var input = "1\n100000\n12\n10\n\n4\n";
            var reader = new StringReader(input);
            Console.SetIn(reader);

            consoleInterface.Run();

            mockCreditCalculator.Verify(x => x.Calculate(It.IsAny<CreditData>()), Times.Once);
        }

        [Fact]
        public void CurrencyConverter_Convert_Called_When_User_Selects_Currency_Option()
        {
            var mockCreditCalculator = new Mock<ICreditCalculator>();
            var mockCurrencyConverter = new Mock<ICurrencyConverter>();
            var mockDepositCalculator = new Mock<IDepositCalculator>();

            mockCurrencyConverter.Setup(x => x.Convert(It.IsAny<CurrencyData>()))
                .Returns(9000.0);

            var consoleInterface = new ConsoleInterface(
                mockCreditCalculator.Object,
                mockCurrencyConverter.Object,
                mockDepositCalculator.Object,
                skipClear: true,
                skipWait: true
            );

            var input = "2\nUSD\nRUB\n100\n4\n";
            var reader = new StringReader(input);
            Console.SetIn(reader);

            consoleInterface.Run();

            mockCurrencyConverter.Verify(x => x.Convert(It.IsAny<CurrencyData>()), Times.Once);
        }

        [Fact]
        public void DepositCalculator_Calculate_Called_When_User_Selects_Deposit_Option()
        {
            var mockCreditCalculator = new Mock<ICreditCalculator>();
            var mockCurrencyConverter = new Mock<ICurrencyConverter>();
            var mockDepositCalculator = new Mock<IDepositCalculator>();

            mockDepositCalculator.Setup(x => x.Calculate(It.IsAny<DepositData>()))
                .Returns((8000.0, 108000.0));

            var consoleInterface = new ConsoleInterface(
                mockCreditCalculator.Object,
                mockCurrencyConverter.Object,
                mockDepositCalculator.Object,
                skipClear: true,
                skipWait: true
            );

            var input = "3\n50000\n6\n8\n1\n\n4\n";
            var reader = new StringReader(input);
            Console.SetIn(reader);

            consoleInterface.Run();

            mockDepositCalculator.Verify(x => x.Calculate(It.IsAny<DepositData>()), Times.Once);
        }
    }
}
