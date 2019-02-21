using PaySlipGenerator;
using PaySlipGenerator.Tax;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace PaySlipGeneratorTest
{
    public class TaxCalculatorTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 18200)]
        [InlineData(0.19, 18201)]
        [InlineData(0.19, 37000)]
        [InlineData(0.325, 37001)]
        [InlineData(0.325, 87000)]
        [InlineData(0.37, 87001)]
        [InlineData(0.37, 180000)]
        [InlineData(0.45, 180001)]
        [InlineData(0.45, 99999999)]
        public void GetTaxBand_ValidTaxBand_ReturnsExpectedVariableTax(double expectedVariableTax, uint annualIncome)
        {
            Assert.Equal(expectedVariableTax, TaxCalculator.GetTaxBand(annualIncome).VariableTax);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 18200)]
        [InlineData(0, 18201)]
        [InlineData(0, 37000)]
        [InlineData(3572, 37001)]
        [InlineData(3572, 87000)]
        [InlineData(19822, 87001)]
        [InlineData(19822, 180000)]
        [InlineData(54232, 180001)]
        [InlineData(54232, 99999999)]
        public void GetTaxBand_ValidTaxBand_ReturnsExpectedFlatTax(double? expectedFlatTax, uint annualIncome)
        {
            Assert.Equal(expectedFlatTax, TaxCalculator.GetTaxBand(annualIncome).FlatTax);
        }

        [Theory]
        [InlineData(5004, 60050)]
        [InlineData(5004, 60045)]
        [InlineData(5003, 60030)]
        [InlineData(5002, 60029)]
        public void GrossIncome_ValidInput_ReturnsExpectedGrossIncome(uint expected, uint annualIncome)
        {
            Assert.Equal(expected, TaxCalculator.GrossIncome(annualIncome));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 18200)]
        [InlineData(0.19, 18201)]
        [InlineData(298, 37000)]
        [InlineData(298, 37001)]
        [InlineData(922, 60050)]
        [InlineData(1652, 87000)]
        [InlineData(1652, 87001)]
        [InlineData(2669, 120000)]
        [InlineData(4519, 180000)]
        [InlineData(4519, 180001)]
        [InlineData(372769, 9999999)]
        public void IncomeTax_ValidInput_ReturnsExpectedGrossIncome(uint expected, uint annualIncome)
        {
            Assert.Equal(expected, TaxCalculator.IncomeTax(annualIncome));
        }

        [Theory]
        [InlineData(4082, 60050)]
        [InlineData(7331, 120000)]
        public void NetIncome_ValidInput_ReturnsExpectedNetIncome(uint expected, uint annualIncome)
        {
            Assert.Equal(expected, TaxCalculator.NetIncome(annualIncome));
        }

        [Theory]
        [InlineData(450, 60050, 0.09)]
        [InlineData(1000, 120000, 0.1)]
        [InlineData(401, 60084, 0.08)]
        public void Super_ValidInput_ReturnsExpectedSuper(uint expected, uint annualIncome, double super)
        {
            Assert.Equal(expected, TaxCalculator.Super(annualIncome, super));
        }

        [Fact]
        public void Super_SuperIsNegative_FailsWithNegativeNumberException()
        {
            Assert.Throws<NegativeNumberException>(() => TaxCalculator.Super(60050, -0.09));
        }
    }
}
