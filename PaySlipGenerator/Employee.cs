using System;
using System.Collections.Generic;
using System.Linq;
using PaySlipGenerator.Exceptions;
using PaySlipGenerator.Tax;
using Serilog;

namespace PaySlipGenerator
{
    public class Employee : IEquatable<object>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public uint AnnualIncome { get; set; }
        public double SuperRate { get; set; }

        public ICollection<PaySlip> Payslips { get; set; }
        private static ILogger Logger;


        public Employee(string firstName, string lastName, uint annualIncome, double superRate)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AnnualIncome = annualIncome;
            this.SuperRate = superRate;

            this.Payslips = new List<PaySlip>();
        }

        public Employee(string firstName, string lastName, uint annualIncome, double superRate, ICollection<PaySlip> payslips)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AnnualIncome = annualIncome;
            this.SuperRate = superRate;

            foreach(PaySlip p in payslips)
            {
                p.Employee = this;
            }

            this.Payslips = (payslips != null) ? payslips : new List<PaySlip>();
        }

        public static void SetLogger(ILogger logger)
        {
            Logger = logger;
        }



        public void GeneratePayslips()
        {
            foreach(PaySlip p in this.Payslips)
            {
                if(!p.Generated)
                {
                    try
                    {
                        p.Generate(this.AnnualIncome, this.SuperRate);
                    }
                    catch (NegativeNumberException ex)
                    {
                        Logger.Error(string.Format("Negative Number error while generating payslip for {0} {1} : {2}", p.Employee.FirstName, p.Employee.LastName, ex.Message));
                    }
                    catch (PayslipGenerationException ex)
                    {
                        Logger.Error(string.Format("Error generating payslip for {0} {1} : {2}", p.Employee.FirstName, p.Employee.LastName, ex.Message));
                    }
                }
            }
        }

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

            if ( !(AnnualIncome.Equals(emp.AnnualIncome)))
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

        public string ToCsvString()
        {
            PaySlip p = this.Payslips.LastOrDefault();
            return string.Format("{0} {1},{2} - {3},{4},{5},{6},{7}", this.FirstName, this.LastName, p.StartDate.ToString("dd MMMM"), p.EndDate.ToString("dd MMMM"), p.GrossIncome, p.IncomeTax, p.NetIncome, p.Super);
        }

        public override int GetHashCode()
        {
            int hash = 11;
            hash+= this.FirstName.GetHashCode() * 7;
            hash += this.LastName.GetHashCode() * 7;
            hash += this.AnnualIncome.GetHashCode() * 7;
            hash += this.SuperRate.GetHashCode() * 7;
            return hash;
        }
    }
}
