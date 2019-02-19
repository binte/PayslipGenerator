using PaySlipGenerator.Tax;
using System;

namespace PaySlipGenerator
{
    public class PaySlip : IEquatable<object>
    {
        public PaySlip(DateTime startDate, DateTime endDate)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public PaySlip(DateTime startDate, DateTime endDate, int grossIncome, int incomeTax, int netIncome, double super)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.GrossIncome = grossIncome;
            this.IncomeTax = incomeTax;
            this.NetIncome = netIncome;
            this.Super = super;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int  GrossIncome { get; set; }
        public int IncomeTax { get; set; }
        public int NetIncome { get; set; }
        public double Super { get; set; }

        public Employee Employee { get; set; }


        public void Generate(int annualIncome, double super)
        {
            this.GrossIncome = TaxCalculator.GrossIncome(annualIncome);
            this.IncomeTax = TaxCalculator.IncomeTax(annualIncome);
            this.NetIncome = this.GrossIncome - this.IncomeTax;
            this.Super = TaxCalculator.Super(annualIncome, super);
        }


        public override bool Equals(object other)
        {
            if (other is null || !(other is PaySlip))
            {
                return false;
            }

            PaySlip p = (PaySlip)other;

            return ( ( (this.Employee == null && p.Employee == null) || (this.Employee != null && p.Employee != null && this.Employee.FirstName == p.Employee.FirstName) ) &&
                    this.StartDate == p.StartDate && this.EndDate == p.EndDate &&
                    this.GrossIncome == p.GrossIncome &&
                    this.IncomeTax == p.IncomeTax && 
                    this.NetIncome == p.NetIncome && 
                    this.Super == p.Super);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartDate, EndDate);
        }
    }
}
