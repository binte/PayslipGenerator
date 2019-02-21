using PaySlipGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PaySlipGeneratorTest
{
    public class EmployeeTest
    {
        [Fact]
        public void GeneratePayslips_EmployeeHasNoPayslips_NoChangesMade()
        {
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09), e2 = new Employee("David", "Rudd", 60050, 0.09);
            e1.GeneratePayslips();
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

            e1.GeneratePayslips();
            Assert.False(e1.Equals(e2));
        }

        [Fact]
        public void GeneratePayslips_EmployeeHasNonGeneratedPayslip_PayslipGeneratedV1()
        {
            PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31)),
                    p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                     e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p2 });

            e1.GeneratePayslips();
            Assert.False(e1.Equals(e2));
        }

        [Fact]
        public void GeneratePayslips_EmployeeHasNonGeneratedPayslip_PayslipGeneratedV2()
        {
            PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31)),
                    p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450);
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                     e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p2 });

            e1.GeneratePayslips();
            Assert.True(e1.Equals(e2));
        }

        [Fact]
        public void GeneratePayslips_EmployeeHasGeneratedPayslip_NoChangesMade()
        {
            PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450),
                    p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31), 5004, 922, 4082, 450);
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p1 }),
                     e2 = new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip> { p2 });

            e1.GeneratePayslips();
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

            e1.GeneratePayslips();
            Assert.True(e1.Equals(e2));
        }



        [Fact]
        public void Equals_ObjectIsNull_ReturnsFalse()
        {
            Employee e = new Employee("David", "Rudd", 60050, 0.09);
            Assert.False(e.Equals(null));
        }

        [Fact]
        public void Equals_ObjectIsAnObjectInstance_ReturnsFalse()
        {
            Employee e = new Employee("David", "Rudd", 60050, 0.09);
            Assert.False(e.Equals(new object()));
        }

        [Fact]
        public void Equals_ObjectIsNotEmployee_ReturnsFalse()
        {
            Employee e = new Employee("David", "Rudd", 60050, 0.09);
            PaySlip p = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            Assert.False(e.Equals(p));
        }

        [Theory]
        [InlineData("Davide")]
        [InlineData("Davi")]
        public void Equals_DifferentFirstName_ReturnsFalse(string firstName)
        {
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09);
            Employee e2 = new Employee(firstName, "Rudd", 60050, 0.09);
            Assert.False(e1.Equals(e2));
        }

        [Theory]
        [InlineData("Rud")]
        [InlineData("Ruddy")]
        public void Equals_DifferentLastName_ReturnsFalse(string lastName)
        {
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09);
            Employee e2 = new Employee("David", lastName, 60050, 0.09);
            Assert.False(e1.Equals(e2));
        }

        [Theory]
        [InlineData(60051)]
        [InlineData(60049)]
        public void Equals_DifferentAnnualIncome_ReturnsFalse(uint annualIncome)
        {
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09);
            Employee e2 = new Employee("David", "Rudd", annualIncome, 0.09);
            Assert.False(e1.Equals(e2));
        }

        [Theory]
        [InlineData(0.091)]
        [InlineData(0.089)]
        [InlineData(0.08)]
        [InlineData(-0.09)]
        public void Equals_DifferentSuper_ReturnsFalse(double super)
        {
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09);
            Employee e2 = new Employee("David", "Rudd", 60050, super);
            Assert.False(e1.Equals(e2));
        }

        [Fact]
        public void Equals_DifferentPayslips_ReturnsFalse()
        {
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09);
            PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            e1.Payslips.Add(p1);

            Employee e2 = new Employee("David", "Rudd", 60050, 0.09);
            PaySlip p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 2), new DateTime(DateTime.Today.Year, 3, 31));
            e2.Payslips.Add(p2);

            Assert.False(e1.Equals(e2));
        }

        [Fact]
        public void Equals_DifferentNumberOfPayslips_ReturnsFalse()
        {
            Employee e1 = new Employee("David", "Rudd", 60050, 0.09);
            PaySlip p1 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            e1.Payslips.Add(p1);

            Employee e2 = new Employee("David", "Rudd", 60050, 0.09);
            PaySlip p2 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 1), new DateTime(DateTime.Today.Year, 3, 31));
            e2.Payslips.Add(p2);
            PaySlip p3 = new PaySlip(new DateTime(DateTime.Today.Year, 3, 2), new DateTime(DateTime.Today.Year, 3, 31));
            e2.Payslips.Add(p3);

            Assert.False(e1.Equals(e2));
        }
    }
}
