/**
 * More info on Collection Fixtures, which I used for sharing common variables in groups of unit tests
 * 
 * https://xunit.github.io/docs/shared-context
 */

using PaySlipGenerator.BLL.Services.Implementation;
using System.IO.Abstractions.TestingHelpers;
using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;
using Xunit;

namespace PaySlipGeneratorTest.BLL.Services
{
    public class PaySlipGeneratorTest
    {
        public class Fixture
        {
            public PayslipGenerator Generator { get; private set; }

            public Fixture()
            {
                this.Generator = new PayslipGenerator();

                // configure logging
                Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs-test.txt")
                .WriteTo.Console()
                .CreateLogger();
            }
        }

        [CollectionDefinition("Common Variables Payslip Generator Test")]
        public class DumyCollection : ICollectionFixture<Fixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }

        [Collection("Common Variables Payslip Generator Test")]
        public class GeneratePayslipUnitTests
        {
            private readonly Fixture Fixture;
            
            public GeneratePayslipUnitTests(Fixture fixture)
            {
                this.Fixture = fixture;
            }


            [Fact]
            public void GeneratePayslips_EmployeeHasNoPayslips_NoChangesMade()
            {
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09), e2 = new Employee("David", "Rudd", 60050, 0.09);
                this.Fixture.Generator.GeneratePayslips(e1, new TaxCalculator(), Log.Logger);
                Assert.True(e1.Equals(e2));
            }

            [Theory]
            [InlineData(5005, 922, 4082, 450)]
            [InlineData(5004, 921, 4082, 450)]
            [InlineData(5004, 922, 4081, 450)]
            [InlineData(5004, 922, 4082, 451)]
            public void GeneratePayslips_EmployeeHasNonGeneratedPayslip_PayslipGeneratedResultsInDifferentEmployees(uint grossIncome, uint incomeTax, uint netIncome, uint super)
            {
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31)),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), grossIncome, incomeTax, netIncome, super);
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p2 });

                this.Fixture.Generator.GeneratePayslips(e1, new TaxCalculator(), Log.Logger);
                Assert.False(e1.Equals(e2));
            }

            [Fact]
            public void GeneratePayslips_EmployeeHasNonGeneratedPayslip_PayslipGeneratedV1()
            {
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31)),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p2 });

                this.Fixture.Generator.GeneratePayslips(e1, new TaxCalculator(), Log.Logger);
                Assert.False(e1.Equals(e2));
            }

            [Fact]
            public void GeneratePayslips_EmployeeHasNonGeneratedPayslip_PayslipGeneratedV2()
            {
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31)),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450);
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p2 });

                this.Fixture.Generator.GeneratePayslips(e1, new TaxCalculator(), Log.Logger);
                Assert.True(e1.Equals(e2));
            }

            [Fact]
            public void GeneratePayslips_EmployeeHasGeneratedPayslip_NoChangesMade()
            {
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450);
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p2 });

                this.Fixture.Generator.GeneratePayslips(e1, new TaxCalculator(), Log.Logger);
                Assert.True(e1.Equals(e2));
            }

            [Fact]
            public void GeneratePayslips_EmployeeHasGeneratedAndUngeneratedPayslips_UngeneratedPayslipGenerated()
            {
                PaySlip p11 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450),
                        p12 = new PaySlip(new DateTime(DateTime.Today.Year, 4, 1), new DateTime(DateTime.Today.Year, 4, 30)),
                        p21 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450),
                        p22 = new PaySlip(new DateTime(DateTime.Today.Year, 4, 1), new DateTime(DateTime.Today.Year, 4, 30), 5004, 922, 4082, 450);
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p11, p12 }),
                         e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p21, p22 });

                this.Fixture.Generator.GeneratePayslips(e1, new TaxCalculator(), Log.Logger);
                Assert.True(e1.Equals(e2));
            }

            [Theory]
            [InlineData(60050, 0.09, 5004, 922, 4082, 450)]
            [InlineData(120000, 0.1, 10000, 2669, 7331, 1000)]
            public void Generate_ValidInput_UpdatesPayslipAccordingly(uint annualIncome, double superRate, uint grossIncome, uint incomeTax, uint netIncome, uint super)
            {
                DateTime beginning = new DateTime(DateTime.Today.Year, 3, 1),
                         end = new DateTime(DateTime.Today.Year, 3, 31);
                PaySlip p1 = new PaySlip(beginning, end, grossIncome, incomeTax, netIncome, super),
                        p2 = new PaySlip(beginning, end);

                this.Fixture.Generator.GeneratePayslip(new TaxCalculator(), p2, annualIncome, superRate);
                Assert.Equal(p1, p2);
            }

            [Theory]
            [InlineData(60050, -0.09, 5004, 922, 4082, 450)]
            [InlineData(120000, -0.1, 10000, 2669, 7331, 1000)]
            public void Generate_NegativeSuper_ThrowsNegativeNumberException(uint annualIncome, double superRate, uint grossIncome, uint incomeTax, uint netIncome, uint super)
            {
                DateTime beginning = new DateTime(DateTime.Today.Year, 3, 1),
                         end = new DateTime(DateTime.Today.Year, 3, 31);
                PaySlip p = new PaySlip(beginning, end, grossIncome, incomeTax, netIncome, super);

                Assert.Throws<NegativeNumberException>(() => this.Fixture.Generator.GeneratePayslip(new TaxCalculator(), p, annualIncome, superRate));
            }
        }
    }
}
