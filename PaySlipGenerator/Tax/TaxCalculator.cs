using System;
using System.Collections.Generic;

namespace PaySlipGenerator.Tax
{
    public static partial class TaxCalculator
    {
        public static uint GrossIncome(uint annualIncome)
        {
            return (uint) Math.Round((double)annualIncome / 12, 0, MidpointRounding.AwayFromZero);
        }

        public static uint IncomeTax(uint annualIncome)
        {
            TaxBand band = TaxBands.GetTaxBand(annualIncome);
            double value = band.VariableTax * (annualIncome - band.TaxableIncomeLB - 1) + band.FlatTax;

            return (uint)Math.Round((double)value / 12, 0, MidpointRounding.AwayFromZero);
        }

        public static uint NetIncome(uint annualIncome)
        {
            return GrossIncome(annualIncome) - IncomeTax(annualIncome);
        }

        public static uint Super(uint annualIncome, double superRate)
        {
            if (superRate < 0)
            {
                throw new NegativeNumberException();
            }

            return (uint)Math.Round((double)GrossIncome(annualIncome) * superRate, 0, MidpointRounding.AwayFromZero);
        }

        public static TaxBand GetTaxBand(uint annualIncome)
        {
            return TaxBands.GetTaxBand(annualIncome);
        }
    }
}
