using PaySlipGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace PaySlipGeneratorTest
{
    public class IntegrationTests
    {
        private readonly ITestOutputHelper output;

        public IntegrationTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ReadEmployeeData_WrongPath_FailsWithFileNotFoundException()
        {
            List<Employee> actual = new List<Employee>(), expected = new List<Employee>();
            Assert.Throws<FileNotFoundException>(() => IO.ReadEmployeeData(@"input.csv"));
        }

        [Fact]
        public void ReadEmployeeData_ReadTwoEmployees_ReturnsSameEmployeeList()  
        {
            List<Employee> actual = IO.ReadEmployeeData(@"data\input.csv"), 
                           expected = new List<Employee>();

            expected.AddRange( new List<Employee>()
            {
                new Employee("David", "Rudd", 60050, 9, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) }),
                new Employee("Ryan", "Chen", 120000, 10, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) })
            });

            Assert.Equal(expected, actual);
        }
    }
}
