﻿using PaySlipGenerator;
using PaySlipGenerator.Tax;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace PaySlipGeneratorTest
{
    public class IOTest
    {
        [Fact]
        public void ParseEmployeeLine_IncorrectlyFormattedPeriod_FailsWithFormatException()
        {
            Employee expected = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) });
            Assert.Throws<FormatException>(() => IO.ParseEmployeeLine("David,Rudd,60050,9%,01 March . 31 March"));
        }

        [Fact]
        public void ParseEmployeeLine_CorrectlyFormattedLine_ReturnsExpectedEmployee()
        {
            Employee expected = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) });
            Assert.True(expected.Equals(IO.ParseEmployeeLine("David,Rudd,60050,9%,01 March - 31 March")));
        }

        [Fact]
        public void ReadEmployeeData_WrongPath_FailsWithFileNotFoundException()
        {
            List<Employee> actual = new List<Employee>(), expected = new List<Employee>();
            Assert.Throws<FileNotFoundException>(() => IO.ReadEmployeeData(@"input.csv"));
        }

        /**
         * INTEGRATION TESTS
         */
        [Fact]
        public void ReadEmployeeData_ReadTwoEmployees_ReturnsSameEmployeeList()
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
    }
}
