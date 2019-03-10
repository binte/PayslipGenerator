using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace PaySlipGeneratorTest.DAL.Models
{
    public class PaySlipTest
    {
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

        [Theory]
        [InlineData("20180302")]
        [InlineData("20190201")]
        public void Equals_DifferentStartDate_ReturnsFalse(string startDate)
        {
            DateTime endDate = new DateTime(DateTime.Today.Year, 3, 31);
            PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), endDate, 5004, 922, 4082, 450);
            PaySlip p2 = new PaySlip(DateTime.ParseExact(startDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), endDate, 5004, 922, 4082, 450);
            Assert.False(p1.Equals(p2));
        }

        [Theory]
        [InlineData("20180330")]
        [InlineData("20190228")]
        public void Equals_DifferentEndDate_ReturnsFalse(string endDate)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, 3, 1);
            PaySlip p1 = new PaySlip(startDate, new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450);
            PaySlip p2 = new PaySlip(startDate, DateTime.ParseExact(endDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), 5004, 922, 4082, 450);
            Assert.False(p1.Equals(p2));
        }

        [Theory]
        [InlineData(5003)]
        [InlineData(5005)]
        public void Equals_DifferentGrossIncome_ReturnsFalse(uint grossIncome)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, 3, 1);
            DateTime endDate = new DateTime(DateTime.Today.Year, 3, 31);
            PaySlip p1 = new PaySlip(startDate, endDate, 5004, 922, 4082, 450);
            PaySlip p2 = new PaySlip(startDate, endDate, grossIncome, 922, 4082, 450);
            Assert.False(p1.Equals(p2));
        }

        [Theory]
        [InlineData(921)]
        [InlineData(923)]
        public void Equals_DifferentIncomeTax_ReturnsFalse(uint incomeTax)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, 3, 1);
            DateTime endDate = new DateTime(DateTime.Today.Year, 3, 31);
            PaySlip p1 = new PaySlip(startDate, endDate, 5004, 922, 4082, 450);
            PaySlip p2 = new PaySlip(startDate, endDate, 5004, incomeTax, 4082, 450);
            Assert.False(p1.Equals(p2));
        }

        [Theory]
        [InlineData(4083)]
        [InlineData(4081)]
        public void Equals_DifferentNetIncome_ReturnsFalse(uint netincome)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, 3, 1);
            DateTime endDate = new DateTime(DateTime.Today.Year, 3, 31);
            PaySlip p1 = new PaySlip(startDate, endDate, 5004, 922, 4082, 450);
            PaySlip p2 = new PaySlip(startDate, endDate, 5004, 922, netincome, 450);
            Assert.False(p1.Equals(p2));
        }

        [Theory]
        [InlineData(451)]
        [InlineData(449)]
        public void Equals_DifferentSuper_ReturnsFalse(uint super)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, 3, 1);
            DateTime endDate = new DateTime(DateTime.Today.Year, 3, 31);
            PaySlip p1 = new PaySlip(startDate, endDate, 5004, 922, 4082, 450);
            PaySlip p2 = new PaySlip(startDate, endDate, 5004, 922, 4082, super);
            Assert.False(p1.Equals(p2));
        }
    }
}
