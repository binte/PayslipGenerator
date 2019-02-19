using System;
using System.Collections.Generic;

namespace PaySlipGenerator.Tax
{
    public static partial class TaxCalculator
    {
        public static int GrossIncome(int annualIncome)
        {
            if (annualIncome<0)
            {
                throw new NegativeNumberException();
            }

            return (int) Math.Round((double)annualIncome / 12, 0, MidpointRounding.AwayFromZero);
        }

        public static int IncomeTax(int annualIncome)
        {
            if (annualIncome < 0)
            {
                throw new NegativeNumberException();
            }

            TaxBand band = TaxBands.GetTaxBand(annualIncome);
            double value = band.VariableTax * (annualIncome - band.TaxableIncomeLB - 1) + band.FlatTax;

            return (int)Math.Round((double)value / 12, 0, MidpointRounding.AwayFromZero);
        }

        public static int NetIncome(int annualIncome)
        {
            return GrossIncome(annualIncome) - IncomeTax(annualIncome);
        }

        public static int Super(int annualIncome, double super)
        {
            return (int)Math.Round((double)GrossIncome(annualIncome) * super, 0, MidpointRounding.AwayFromZero);
        }

        public static TaxBand GetTaxBand(int annualIncome)
        {
            return TaxBands.GetTaxBand(annualIncome);
        }
    }
}
