using PaySlipGenerator;
using PaySlipGenerator.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace PaySlipGeneratorTest
{
    public class IOTest
    {
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

        [Fact]
        public void ReadEmployeeData_MockReadTwoEmployees_ReturnsSameEmployeeList()
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("David,Rudd,60050,9 %,01 March - 31 March\r\nRyan,Chen,120000,10 %,01 March - 31 March");

            mockFileSystem.AddFile(@"data\input.csv", mockInputFile);
            IO.SetFileSystem(mockFileSystem);  // inject the mock FS into the IO class
            List<Employee> actual = IO.ReadEmployeeData(@"data\input.csv"),
               expected = new List<Employee>();


            expected.AddRange(new List<Employee>()
            {
                new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) }),
                new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) })
            });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadEmployeeData_MockWrongParameterNumber_FailsAndOutputsToConsoleError()
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("Willy,60000,8%,01 March - 31 March");

            mockFileSystem.AddFile(@"data\input-error-parameterNumber.csv", mockInputFile);
            IO.SetFileSystem(mockFileSystem);  // inject the mock FS into the IO class

            using (StringWriter sw = new StringWriter())
            {
                Console.SetError(sw);
                object objs = IO.ReadEmployeeData(@"data\input-error-parameterNumber.csv");
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Wrong parameter number", "Willy,60000,8%,01 March - 31 March");
                Assert.Equal(output, sw.ToString().TrimEnd('\r', '\n'));
                Console.SetOut(new StreamWriter(Console.OpenStandardError()));
            }
        }

        [Fact]
        public void ReadEmployeeData_MockWrongAnnualIncome_FailsAndOutputsToConsoleError()
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("Mauro,Coutinho,70000.5,9%,01 March - 31 March");

            mockFileSystem.AddFile(@"data\input-error-annualIncome.csv", mockInputFile);
            IO.SetFileSystem(mockFileSystem);  // inject the mock FS into the IO class

            using (StringWriter sw = new StringWriter())
            {
                Console.SetError(sw);
                object objs = IO.ReadEmployeeData(@"data\input-error-annualIncome.csv");
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Annual income in an incorrect format", "Mauro,Coutinho,70000.5,9%,01 March - 31 March");
                Assert.Equal(output, sw.ToString().TrimEnd('\r', '\n'));
                Console.SetOut(new StreamWriter(Console.OpenStandardError()));
            }
        }

        [Fact]
        public void ReadEmployeeData_MockWrongSuper_FailsAndOutputsToConsoleError()
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("Bruno,Cunha,75000,-1%,01 March - 31 March");

            mockFileSystem.AddFile(@"data\input-error-super.csv", mockInputFile);
            IO.SetFileSystem(mockFileSystem);  // inject the mock FS into the IO class

            using (StringWriter sw = new StringWriter())
            {
                Console.SetError(sw);
                object objs = IO.ReadEmployeeData(@"data\input-error-super.csv");
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Super in an incorrect format", "Bruno,Cunha,75000,-1%,01 March - 31 March");
                Assert.Equal(output, sw.ToString().TrimEnd('\r', '\n'));
                Console.SetOut(new StreamWriter(Console.OpenStandardError()));
            }
        }

        [Fact]
        public void ReadEmployeeData_MockSuperOutBounds_FailsAndOutputsToConsoleError()
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("Carlos,Salvador,81000,51%,01 March - 31 March");

            mockFileSystem.AddFile(@"data\input-error-superOutOfBounds.csv", mockInputFile);
            IO.SetFileSystem(mockFileSystem);  // inject the mock FS into the IO class

            using (StringWriter sw = new StringWriter())
            {
                Console.SetError(sw);
                object objs = IO.ReadEmployeeData(@"data\input-error-superOutOfBounds.csv");
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Super out of bounds [0-50]%", "Carlos,Salvador,81000,51%,01 March - 31 March");
                Assert.Equal(output, sw.ToString().TrimEnd('\r', '\n'));
                Console.SetOut(new StreamWriter(Console.OpenStandardError()));
            }
        }

        [Fact]
        public void ReadEmployeeData_IncorrectlyFormattedPeriod_FailsAndOutputsToConsoleError()
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("Vitor,Peru,75000,7%,01 March");

            mockFileSystem.AddFile(@"data\input-error-period.csv", mockInputFile);
            IO.SetFileSystem(mockFileSystem);  // inject the mock FS into the IO class

            using (StringWriter sw = new StringWriter())
            {
                Console.SetError(sw);
                object objs = IO.ReadEmployeeData(@"data\input-error-period.csv");
                string output = string.Format("Error in line {0} : {1}. || Line: {2}", 1, "Date Interval in an incorrect format", "Vitor,Peru,75000,7%,01 March");
                Assert.Equal(output, sw.ToString().TrimEnd('\r', '\n'));
                Console.SetOut(new StreamWriter(Console.OpenStandardError()));
            }
        }

        [Fact]
        public void WriteToStream_OneEmployee_Success()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                List<Employee> employees = new List<Employee>() { new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31), 5004, 922, 4082, 450) }), };

                IO.WriteToStream(employees, writer);

                string actual = Encoding.UTF8.GetString(stream.ToArray());
                Assert.Equal("David Rudd,01 March - 31 March,5004,922,4082,450\r\n", actual);
            }
        }

        /**
         * INTEGRATION TESTS
         */
        [Fact]
        public void ReadEmployeeData_WrongPath_FailsWithFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => IO.ReadEmployeeData(@"input.csv"));
        }

        [Fact]
        public void ReadToStream_ReadTwoEmployees_ReturnsSameEmployeeList()
        {
            List<Employee> actual = IO.ReadEmployeeData(@"data\input.csv"),
                           expected = new List<Employee>();

            expected.AddRange(new List<Employee>()
            {
                new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) }),
                new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) })
            });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteToFile_OneEmployeeInStream_Success()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                List<Employee> before = new List<Employee>() { new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31), 5004, 922, 4082, 450) }), },
                               after = new List<Employee>();

                IO.WriteToStream(before, writer);
                IO.WriteToFile(@"data\payslips.csv", stream);
            }

            string line = "";
            using (var reader = new StreamReader(File.OpenRead(@"data\payslips.csv")))
            {
                line = reader.ReadLine();
            }

            Assert.Equal("David Rudd,01 March - 31 March,5004,922,4082,450", line);
        }
    }
}
