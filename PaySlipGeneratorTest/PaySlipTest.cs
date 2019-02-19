using PaySlipGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace PaySlipGeneratorTest
{
    public class PaySlipTest
    {
        private readonly ITestOutputHelper output;

        public PaySlipTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(60050, 0.09, 5004, 922, 4082, 450)]
        [InlineData(120000, 0.1, 10000, 2669, 7331, 1000)]
        public void GeneratePayslip_ValidInput_UpdatesPayslipAccordingly(int annualSalary, double superRate, int grossIncome, int incomeTax, int netIncome, double super)
        {
            DateTime beginning = new DateTime(DateTime.Today.Year, 3, 1),
                     end = new DateTime(DateTime.Today.Year, 3, 31);
            PaySlip p1 = new PaySlip(beginning, end, grossIncome, incomeTax, netIncome, super),
                    p2 = new PaySlip(beginning, end);

            p2.Generate(annualSalary, superRate);
            Assert.Equal(p1, p2);
        }
    }
}
