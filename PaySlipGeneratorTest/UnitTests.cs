using PaySlipGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace PaySlipGeneratorTest
{
    public class UnitTests
    {
        private readonly ITestOutputHelper output;

        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ParseEmployeeLine_IncorrectlyFormattedPeriod_FailsWithFormatException()
        {
            Employee expected = new Employee("David", "Rudd", 60050, 9, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) });
            Assert.Throws<FormatException>(() => IO.ParseEmployeeLine("David,Rudd,60050,9%,01 March . 31 March"));
        }

        [Fact]
        public void ParseEmployeeLine_CorrectlyFormattedLine_ReturnsExpectedEmployee()
        {
            Employee expected = new Employee("David", "Rudd", 60050, 9, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) });
            Assert.True(expected.Equals(IO.ParseEmployeeLine("David,Rudd,60050,9%,01 March - 31 March")));
        }

        [Theory]
        [InlineData(5004, 60050)]
        [InlineData(5004, 60045)]
        [InlineData(5003, 60030)]
        [InlineData(5002, 60029)]
        public void GrossIncome_InputWithoutDecimalPart_ReturnsExpectedGrossIncome(int expected, int value)
        {
            Assert.Equal(expected, TaxCalculator.GrossIncome(value));
        }

        [Fact]
        public void GrossIncome_InputIsNegative_FailsWithNegativeException()
        {
            Assert.Throws<NegativeNumberException>(() => TaxCalculator.GrossIncome(-60050));
        }
    }
}
