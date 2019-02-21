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

        public PaySlip(DateTime startDate, DateTime endDate, uint grossIncome, uint incomeTax, uint netIncome, uint super)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.GrossIncome = grossIncome;
            this.IncomeTax = incomeTax;
            this.NetIncome = netIncome;
            this.Super = super;
            this.Generated = true;  // If the constructor is called with all these parameters, it is assumed that the payslip has been generated
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public uint GrossIncome { get; set; }
        public uint IncomeTax { get; set; }
        public uint NetIncome { get; set; }
        public uint Super { get; set; }
        public bool Generated { get; set; }  // By default, the payslips won't have been generated (False is the default value already)

        public Employee Employee { get; set; }


        public void Generate(uint annualIncome, double superRate)
        {
            this.GrossIncome = TaxCalculator.GrossIncome(annualIncome);
            this.IncomeTax = TaxCalculator.IncomeTax(annualIncome);
            this.NetIncome = this.GrossIncome - this.IncomeTax;
            this.Super = TaxCalculator.Super(annualIncome, superRate);
            this.Generated = true;
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
