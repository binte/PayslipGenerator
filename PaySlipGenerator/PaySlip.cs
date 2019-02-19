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

            if ( this.StartDate != p.StartDate)
            {
                return false;
            }

            if( this.EndDate != p.EndDate )
            {
                return false;
            }

            if (this.GrossIncome != p.GrossIncome)
            {
                return false;
            }

            if( this.IncomeTax != p.IncomeTax )
            {
                return false;
            }

            if( this.NetIncome != p.NetIncome)
            {
                return false;
            }

            if( this.Super != p.Super )
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartDate, EndDate);
        }
    }
}
