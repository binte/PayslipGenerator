using System;
using System.Collections.Generic;
using System.Text;

namespace PaySlipGenerator
{
    public static class TaxCalculator
    {
        public static int GrossIncome(int annualIncome)
        {
            if(annualIncome<0)
            {
                throw new NegativeNumberException();
            }

            return (int) Math.Round((double)annualIncome / 12, 0, MidpointRounding.AwayFromZero);
        }
    }
}
