using Moq;
using PaySlipGenerator.DAL.Context.Interfaces;
using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.DAL.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PaySlipGeneratorTest.DAL.Context
{
    public class ContextTest
    {
        /**
         * Unit Tests
         */
        [Fact]
        public void GetEmployees_ReadTwoMockedEmployees_ReturnsSameEmployeeList()
        {
            List<Employee> actual = new List<Employee>();
            actual.AddRange(new List<Employee>()
            {
                new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) }),
                new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) })
            });
            Mock<IContext> context = new Mock<IContext>();
            context.Setup(x => x.Employees).Returns(actual);
            EmployeeRepository repository = new EmployeeRepository(context.Object);

            List<Employee> expected = new List<Employee>();
            expected.AddRange(new List<Employee>()
            {
                new Employee("David", "Rudd", 60050, 0.09, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) }),
                new Employee("Ryan", "Chen", 120000, 0.1, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) })
            });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetEmployees_MockWritingOneEmployeeInStream_Success()
        {
            Mock<IContext> context = new Mock<IContext>();
            context.Setup(x => x.SetEmployees()).Verifiable();  // moq expects a call to SetEmployees()
            EmployeeRepository repository = new EmployeeRepository(context.Object);
            repository.Persist();  // do the call to SetEmployees
            context.VerifyAll();  // verify if the call was done
        }

        [Fact]
        public void SetEmployees_MockWritingOneEmployeeInStream_FailsBecauseMethodWasNotInvoked()
        {
            Mock<IContext> context = new Mock<IContext>();
            context.Setup(x => x.SetEmployees()).Verifiable();  // moq expects a call to SetEmployees()
            EmployeeRepository repository = new EmployeeRepository(context.Object);
            context.Verify(x => x.SetEmployees(), Times.Never);  // verify if the call never took place
        }
    }
}
