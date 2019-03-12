using PaySlipGenerator.ApL.Services.Implementation;
using PaySlipGenerator.BLL.Services.Implementation;
using PaySlipGenerator.DAL.Context.Implementation;
using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.DAL.Repository.Implementation;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Moq;
using Xunit;
using PaySlipGenerator.DAL.Context.Interfaces;

namespace PaySlipGeneratorTest.ApL.Services
{
    public class TaxCalculatorServiceTest
    {
        public class Fixture
        {
            public TaxCalculator Calculator { get; private set; }
            public PayslipGenerator Generator { get; private set; }
            public string InputFilePath { get; set; }
            public string OutputFilePath { get; set; }

            public Fixture()
            {

                this.Calculator = new TaxCalculator();
                this.Generator = new PayslipGenerator();
                this.InputFilePath = @"data\input.csv";
                this.OutputFilePath = @"data\output2.csv";

                // configure logging
                Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs-test.txt")
                .WriteTo.Console()
                .CreateLogger();
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
        public class GeneratePayslipsTests
        {
            private readonly Fixture Fixture;

            public GeneratePayslipsTests(Fixture fixture)
            {
                this.Fixture = fixture;
            }

            /**
             * UNIT TESTS
             */
            [Fact]
            public void GeneratePayslips_TwoMockedEmployeesWithUngenerateddPayslips_PayslipsGenerated()
            {
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31)),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip> { p2 });
                Mock<IContext> context = new Mock<IContext>();
                context.Setup(x => x.Employees).Returns(new List<Employee>() { e1, e2 });
                EmployeeRepository repository = new EmployeeRepository(context.Object);

                TaxCalculatorService Manager = new TaxCalculatorService(repository, this.Fixture.Calculator, this.Fixture.Generator, Log.Logger);
                PaySlip pp1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450),
                        pp2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 10000, 2669, 7331, 1000);
                Employee ee1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { pp1 }),
                         ee2 = new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip> { pp2 });
                IEnumerable<Employee> expected = new List<Employee>() { ee1, ee2 };

                Assert.NotEqual(expected, repository.GetAll());
                Manager.GeneratePayslips();
                Assert.Equal(expected, repository.GetAll());
            }

            [Fact]
            public void GeneratePayslips_TwoMockedEmployeesWithUngenerateddPayslips_PayslipsGeneratedButOneIsDifferent()
            {
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31)),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip> { p2 });
                Mock<IContext> context = new Mock<IContext>();
                context.Setup(x => x.Employees).Returns(new List<Employee>() { e1, e2 });
                EmployeeRepository repository = new EmployeeRepository(context.Object);

                TaxCalculatorService Manager = new TaxCalculatorService(repository, this.Fixture.Calculator, this.Fixture.Generator, Log.Logger);
                PaySlip pp1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5005, 922, 4082, 450),
                        pp2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 10000, 2669, 7331, 1000);
                Employee ee1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { pp1 }),
                         ee2 = new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip> { pp2 });
                IEnumerable<Employee> expected = new List<Employee>() { ee1, ee2 };

                Assert.NotEqual(expected, repository.GetAll());
                Manager.GeneratePayslips();
                Assert.NotEqual(expected, repository.GetAll());
            }

            /**
             * INTEGRATION TEST
             */
            [Fact]
            public void GeneratePayslips_TwoEmployeesWithUngenerateddPayslips_PayslipsGenerated()
            {
                EmployeeRepository Repository = new EmployeeRepository(new Context(new FileContext(this.Fixture.InputFilePath, this.Fixture.OutputFilePath, new FileSystem(), Log.Logger)));
                TaxCalculatorService Manager = new TaxCalculatorService(Repository, this.Fixture.Calculator, this.Fixture.Generator, Log.Logger);
                PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450),
                        p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 10000, 2669, 7331, 1000);
                Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                         e2 = new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip> { p2 });
                IEnumerable<Employee> expected = new List<Employee>() { e1, e2 };

                Assert.NotEqual(expected, Repository.GetAll());
                Manager.GeneratePayslips();
                Assert.Equal(expected, Repository.GetAll());
            }

            /**
             * ACCEPTANCE TEST
             */
            [Fact]
            public void StoreData_AddNewEmployee_FileUpdatedSuccessfuly()
            {
                EmployeeRepository Repository = new EmployeeRepository(new Context(new FileContext(this.Fixture.InputFilePath, this.Fixture.OutputFilePath, new FileSystem(), Log.Logger)));
                TaxCalculatorService Manager = new TaxCalculatorService(Repository, this.Fixture.Calculator, this.Fixture.Generator, Log.Logger);
                PaySlip p = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
                Employee e = new Employee("Artur", "Chaves", 90050, 0.11, new List<PaySlip> { p });
                var outputFileContent = "David Rudd,01 March - 31 March,5004,922,4082,450\nRyan Chen,01 March - 31 March,10000,2669,7331,1000";
                var expected = new string[] { "David Rudd,01 March - 31 March,5004,922,4082,450" ,
                                               "Ryan Chen,01 March - 31 March,10000,2669,7331,1000",
                                               "Artur Chaves,01 March - 31 March,7504,1746,5758,825" };

                Repository.GetAll();
                Repository.Add(e);
                Manager.GeneratePayslips();
                Repository.Persist();

                int n = 0;
                using (var reader = new StreamReader((new FileSystem()).File.OpenRead(this.Fixture.OutputFilePath)))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        Assert.Equal(expected[n++], line);
                    }
                }

                // Restore output file content
                using (var writer = new StreamWriter(this.Fixture.OutputFilePath))
                {
                    writer.Write(outputFileContent);
                    writer.Flush();
                }

                // Restore cache to its original state
                Repository.Remove(e);
            }
        }
    }
}
