using System;
using System.Collections.Generic;
using System.Linq;

namespace PaySlipGenerator
{
    public class Employee : IEquatable<object>
    {
        public Employee(string firstName, string lastName, uint annualSalary, double superRate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AnnualSalary = annualSalary;
            this.SuperRate = superRate;

            this.Payslips = new List<PaySlip>();
        }

        public Employee(string firstName, string lastName, uint annualSalary, double superRate, ICollection<PaySlip> payslips)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AnnualSalary = annualSalary;
            this.SuperRate = superRate;

            foreach(PaySlip p in payslips)
            {
                p.Employee = this;
            }

            this.Payslips = payslips;
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public uint AnnualSalary { get; set; }
        public double SuperRate { get; set; }

        public ICollection<PaySlip> Payslips { get; set; }


        public override bool Equals(object other)
        {
            if( other is null || !(other is Employee) )
            {
                return false;
            }

            Employee emp = (Employee)other;

            if ( !FirstName.Equals(emp.FirstName))
            {
                return false;
            }

            if ( !(LastName.Equals(emp.LastName)) )
            {
                return false;
            }

            if ( !(AnnualSalary.Equals(emp.AnnualSalary)))
            {
                return false;
            }

            if ( !(SuperRate.Equals(emp.SuperRate)))
            {
                return false;
            }

            if (this.Payslips.Except(emp.Payslips).Count() > 0 || emp.Payslips.Except(this.Payslips).Count() > 0)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName);
        }

        public string ToCsvString()
        {
            PaySlip p = this.Payslips.LastOrDefault();
            return string.Format("{0} {1},{2} - {3},{4},{5},{6},{7}", this.FirstName, this.LastName, p.StartDate.ToString("dd MMMM"), p.EndDate.ToString("dd MMMM"), p.GrossIncome, p.IncomeTax, p.NetIncome, p.Super);
        }
    }
}
