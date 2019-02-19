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

        [Fact]
        public void Equals_ObjectIsNull_ReturnsFalse()
        {
            PaySlip p = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            Assert.False(p.Equals(null));
        }

        [Fact]
        public void Equals_ObjectIsAnObjectInstance_ReturnsFalse()
        {
            PaySlip p = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            Assert.False(p.Equals(new object()));
        }

        [Fact]
        public void Equals_ObjectIsNotPayslip_ReturnsFalse()
        {
            Employee e = new Employee("David", "Rudd", 60050, 0.09);
            PaySlip p = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            Assert.False(p.Equals(e));
        }

        //[Theory]
        //[InlineData("20190228")]
        //[InlineData("20190302")]
        //[InlineData("20180301")]
        //[InlineData("20200301")]
        //[InlineData("20190201")]
        //[InlineData("20190401")]
        //public void Equals_StartDate_ReturnsFalse(string startDate)
        //{
        //    PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 31));
        //    PaySlip p2 = new PaySlip(firstName, "Rudd", 60050, 0.09);
        //    Assert.False(p1.Equals(p2));
        //}
    }
}
