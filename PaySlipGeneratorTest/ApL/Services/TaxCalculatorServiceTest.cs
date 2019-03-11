using PaySlipGenerator.ApL.Services.Implementation;
using PaySlipGenerator.BLL.Services.Implementation;
using PaySlipGenerator.DAL;
using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.DAL.Repository.Implementation;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using Xunit;

namespace PaySlipGeneratorTest.ApL.Services
{
    public class TaxCalculatorServiceTest
    {
        public class Fixture
        {
            public TaxCalculatorService Manager { get; private set; }
            public TaxCalculator Calculator { get; private set; }
            public PayslipGenerator Generator { get; private set; }
            public EmployeeRepository Repository { get; private set; }

            public Fixture()
            {

                this.Calculator = new TaxCalculator();
                this.Generator = new PayslipGenerator();

                // configure logging
                Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs-test.txt")
                .WriteTo.Console()
                .CreateLogger();

                Context context = new Context(@"data\input.csv", @"data\output.csv", new FileSystem(), Log.Logger);
                Repository = new EmployeeRepository(context);

                Manager = new TaxCalculatorService(Repository, Calculator, Generator, Log.Logger);
            }
        }

        [CollectionDefinition("Common Variables Tax Calculator Service Test")]
        public class DumyCollection : ICollectionFixture<Fixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }

        [Collection("Common Variables Tax Calculator Service Test")]
        public class GeneratePayslipsUnitTests
        {
            private readonly Fixture Fixture;

            public GeneratePayslipsUnitTests(Fixture fixture)
            {
                this.Fixture = fixture;
            }

            /**
             * INTEGRATION TESTS
             */
            [Fact]
            public void GeneratePayslips_TwoEmployeesWithUngenerateddPayslips_PayslipsGenerated()
            {
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 10000, 2669, 7331, 1000);
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip> { p2 });
                IEnumerable<Employee> expected = new List<Employee>() { e1, e2 };

                Assert.NotEqual(expected, this.Fixture.Repository.GetAll());
                this.Fixture.Manager.GeneratePayslips();
                Assert.Equal(expected, this.Fixture.Repository.GetAll());
            }

            // TODO test also StoreData() method in an integration method
        }

    }
}
