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

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override bool Equals(object other)
        {
            if (!(other is PaySlip))
            {
                return false;
            }

            PaySlip p = (PaySlip)other;

            return (this.StartDate == p.StartDate && this.EndDate == p.EndDate);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartDate, EndDate);
        }
    }
}