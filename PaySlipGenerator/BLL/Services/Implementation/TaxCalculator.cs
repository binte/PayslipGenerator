using PaySlipGenerator.BLL.Services.Interfaces;
using PaySlipGenerator.Exceptions;
using System;

/**
 * Domain Service
 */
namespace PaySlipGenerator.BLL.Services.Implementation
{
    public class TaxCalculator : ITaxCalculator
    {
        public uint GrossIncome(uint annualIncome)
        {
            return (uint) Math.Round((double)annualIncome / 12, 0, MidpointRounding.AwayFromZero);
        }

        public uint IncomeTax(uint annualIncome)
        {
            TaxBand band = TaxBands.GetTaxBand(annualIncome);
            double value = band.VariableTax * (annualIncome - band.TaxableIncomeLB - 1) + band.FlatTax;

            return (uint)Math.Round((double)value / 12, 0, MidpointRounding.AwayFromZero);
        }

        public uint NetIncome(uint annualIncome)
        {
            return GrossIncome(annualIncome) - IncomeTax(annualIncome);
        }

        public uint Super(uint annualIncome, double superRate)
        {
            if (superRate < 0)
            {
                throw new NegativeNumberException("Negative value received for super rate @TaxCalculator");
            }

            return (uint)Math.Round((double)GrossIncome(annualIncome) * superRate, 0, MidpointRounding.AwayFromZero);
        }

        public TaxBand GetTaxBand(uint annualIncome)
        {
            return TaxBands.GetTaxBand(annualIncome);
        }
    }
}
