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

        [Fact]
        public void GrossIncome_WithoutDecimalPart_ReturnsExpectedGrossIncome()
        {

        }

        [Fact]
        public void GrossIncome_WithDecimalPart_ReturnsExpectedGrossIncome()
        {

        }

        [Fact]
        public void GrossIncome_NegativeNumber_FAILSSSSSSS()
        {

        }
    }
}
