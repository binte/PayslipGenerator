/**
 * More info on Collection Fixtures, which I used for sharing common variables in groups of unit tests
 * 
 * https://xunit.github.io/docs/shared-context
 */

using Moq;
using PaySlipGenerator.DAL.Context.Implementation;
using PaySlipGenerator.DAL.Context.Interfaces;
using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.DAL.Repository.Implementation;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Xunit;

namespace PaySlipGeneratorTest.DAL.Context
{
    public static class Globals
    {
        public const string InputFilePath = @"data\input.csv";
        public const string OutputFilePath = @"data\output.csv";
    }

    public class ReadWriteTests {

        /**
         * Integration Test
         */
        [Fact]
        public void ReadFile_WrongPath_FailsWithFileNotFoundException()
        {
            FileContext Context = new FileContext("input.csv", Globals.OutputFilePath, new FileSystem(), Log.Logger);

            Assert.Throws<FileNotFoundException>(() => Context.ReadFile());
        }
    }


    public class Fixture
    {
        public MockFileSystem MockFS { get; private set; }

        public Fixture()
        {
            // configure logging
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs-test.txt")
            .WriteTo.Console()
            .CreateLogger();

            this.MockFS = new MockFileSystem();
        }
    }

    [CollectionDefinition("Common Variables FileContext Test")]
    public class DumyCollection : ICollectionFixture<Fixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }


    [Collection("Common Variables FileContext Test")]
    public class ReadWriteUnitTests
    {
        private readonly Fixture Fixture;

        public ReadWriteUnitTests(Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void ReadFile_MockReadTwoEmployees_ReturnsSameEmployeeList()
        {
            var mockInputFile = new MockFileData("David,Rudd,60050,9 %,01 March - 31 March\r\nRyan,Chen,120000,10 %,01 March - 31 March");
            this.Fixture.MockFS.AddFile(Globals.InputFilePath, mockInputFile);

            IList<Employee> actual = new FileContext(Globals.InputFilePath, Globals.OutputFilePath, this.Fixture.MockFS, Log.Logger).ReadFile();
            List<Employee> expected = new List<Employee>();

            expected.AddRange(new List<Employee>()
                {
                    new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) }),
                    new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) })
                });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadFile_MockWrongParameterNumber_FailsAndOutputsToConsoleError()
        {
            var mockInputFile = new MockFileData("Willy,60000,8%,01 March - 31 March");
            this.Fixture.MockFS.AddFile(Globals.InputFilePath, mockInputFile);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                object objs = new FileContext(Globals.InputFilePath, Globals.OutputFilePath, this.Fixture.MockFS, Log.Logger).ReadFile();
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Wrong parameter number", "Willy,60000,8%,01 March - 31 March");
                Assert.Contains(output, sw.ToString());
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        [Fact]
        public void ReadFile_MockWrongAnnualIncome_FailsAndOutputsToConsoleError()
        {
            var mockInputFile = new MockFileData("Mauro,Coutinho,70000.5,9%,01 March - 31 March");
            this.Fixture.MockFS.AddFile(Globals.InputFilePath, mockInputFile);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                object objs = new FileContext(Globals.InputFilePath, Globals.OutputFilePath, this.Fixture.MockFS, Log.Logger).ReadFile();
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Annual income in an incorrect format", "Mauro,Coutinho,70000.5,9%,01 March - 31 March");
                Assert.Contains(output, sw.ToString());
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        [Fact]
        public void ReadFile_MockWrongSuper_FailsAndOutputsToConsoleError()
        {
            var mockInputFile = new MockFileData("Bruno,Cunha,75000,-1%,01 March - 31 March");
            this.Fixture.MockFS.AddFile(Globals.InputFilePath, mockInputFile);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                object objs = new FileContext(Globals.InputFilePath, Globals.OutputFilePath, this.Fixture.MockFS, Log.Logger).ReadFile();
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Super in an incorrect format", "Bruno,Cunha,75000,-1%,01 March - 31 March");
                Assert.Contains(output, sw.ToString());
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        [Fact]
        public void ReadFile_MockSuperOutBounds_FailsAndOutputsToConsoleError()
        {
            var mockInputFile = new MockFileData("Carlos,Salvador,81000,51%,01 March - 31 March");
            this.Fixture.MockFS.AddFile(Globals.InputFilePath, mockInputFile);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                object objs = new FileContext(Globals.InputFilePath, Globals.OutputFilePath, this.Fixture.MockFS, Log.Logger).ReadFile();
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Super out of bounds [0-50]%", "Carlos,Salvador,81000,51%,01 March - 31 March");
                Assert.Contains(output, sw.ToString());
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        [Fact]
        public void ReadFile_IncorrectlyFormattedPeriod_FailsAndOutputsToConsoleError()
        {
            var mockInputFile = new MockFileData("Vitor,Peru,75000,7%,01 March");
            this.Fixture.MockFS.AddFile(Globals.InputFilePath, mockInputFile);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                object objs = new FileContext(Globals.InputFilePath, Globals.OutputFilePath, this.Fixture.MockFS, Log.Logger).ReadFile();
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Date Interval in an incorrect format", "Vitor,Peru,75000,7%,01 March");
                Assert.Contains(output, sw.ToString());
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        [Fact]
        public void WriteStream_OneEmployee_Success()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                List<Employee> employees = new List<Employee>() { new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31), 5004, 922, 4082, 450) }), };

                new FileContext(Globals.InputFilePath, Globals.OutputFilePath, this.Fixture.MockFS, Log.Logger).WriteStream(employees, writer);

                string actual = Encoding.UTF8.GetString(stream.ToArray());
                Assert.Equal("David Rudd,01 March - 31 March,5004,922,4082,450\r\n", actual);
            }
        }
    }



    public class FixtureIntegrationTests
    {
        public FileContext Context { get; private set; }

        public FixtureIntegrationTests()
        {
            // configure logging
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs-test.txt")
            .WriteTo.Console()
            .CreateLogger();

            this.Context = new FileContext(Globals.InputFilePath, Globals.OutputFilePath, new FileSystem(), Log.Logger);
        }
    }

    [CollectionDefinition("Common Variables FileContext Integration Testing")]
    public class CodelessCollection : ICollectionFixture<FixtureIntegrationTests>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }


    [Collection("Common Variables FileContext Integration Testing")]
    public class ReadWriteIntegrationTests
    {
        private readonly FixtureIntegrationTests Fixture;

        public ReadWriteIntegrationTests(FixtureIntegrationTests fixture)
        {
            this.Fixture = fixture;
        }

        /**
         * INTEGRATION TESTS
         */
        [Fact]
        public void ReadStream_ReadTwoEmployees_ReturnsSameEmployeeList()
        {
            IList<Employee> actual = this.Fixture.Context.ReadFile();
            List<Employee> expected = new List<Employee>();

            expected.AddRange(new List<Employee>()
                {
                    new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) }),
                    new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) })
                });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteFile_OneEmployeeInStream_Success()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                List<Employee> employees = new List<Employee>() { new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31), 5004, 922, 4082, 450) }), };

                this.Fixture.Context.WriteStream(employees, writer);
                this.Fixture.Context.WriteFile(stream);
            }

            string line = "";
            using (var reader = new StreamReader(File.OpenRead(Globals.OutputFilePath)))
            {
                line = reader.ReadLine();
            }

            Assert.Equal("David Rudd,01 March - 31 March,5004,922,4082,450", line);
        }
    }


    //[Theory]
    //[InlineData("Willy,60000,8%,01 March - 31 March")]
    //[InlineData("José, Coutinho,85000,01 March - 31 March")]
    //[InlineData("Sandra, Coutinho,10%,01 March - 31 March")]
    //[InlineData("Manuel, Almeida,85000,10%")]
    //public void ParseEmployeeLine_WrongParameterNumber_FailsWithWrongParameterNumberException(string line)
    //{
    //    Assert.Throws<WrongParameterNumberException>(() => IO.ParseEmployeeLine(line));
    //}

    //[Fact]
    //public void ParseEmployeeLine_IncorrectlyFormattedAnnualIncome_FailsWithAnnualIncomeFormatException()
    //{
    //    Assert.Throws<AnnualIncomeFormatException>(() => IO.ParseEmployeeLine("Mauro, Coutinho, 70000.5, 9 %, 01 March - 31 March"));
    //}

    //[Theory]
    //[InlineData("Bruno,Cunha,75000,-1%,01 March - 31 March")]
    //[InlineData("Lígia, Coutinho,50000,,01 March - 31 March")]
    //public void ParseEmployeeLine_IncorrectlyFormattedSuper_FailsWithSuperFormatException(string line)
    //{
    //    Assert.Throws<SuperFormatException>(() => IO.ParseEmployeeLine(line));
    //}

    //[Fact]
    //public void ParseEmployeeLine_SuperOutOfBounds_FailsWithAnnualSuperOutOfBoundsException()
    //{
    //    Assert.Throws<SuperOutOfBoundsException>(() => IO.ParseEmployeeLine("Carlos,Salvador,81000,51%,01 March - 31 March"));
    //}

    //[Theory]
    //[InlineData("David,Rudd,60050,9%,01 March . 31 March")]
    //[InlineData("Vitor,Peru,75000,7%,01 March")]
    //[InlineData("Sónia,Nogueira,75000,10%,")]
    //public void ParseEmployeeLine_IncorrectlyFormattedPeriod_FailsWithDateIntervalFormatException(string line)
    //{
    //    Assert.Throws<DateIntervalFormatException>(() => IO.ParseEmployeeLine(line));
    //}

    //[Fact]
    //public void ParseEmployeeLine_CorrectlyFormattedLine_ReturnsExpectedEmployee()
    //{
    //    Employee expected = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) });
    //    Assert.True(expected.Equals(IO.ParseEmployeeLine("David,Rudd,60050,9%,01 March - 31 March")));
    //}
}
