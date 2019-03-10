using System;
using System.Collections.Generic;

namespace PaySlipGenerator.BLL
{
    public static class TaxBands
    {
        private static List<TaxBand> List = new List<TaxBand>() {
            new TaxBand(0, 18200, 0, 0),
            new TaxBand(18201, 37000, 0.19, 0),
            new TaxBand(37001, 87000, 0.325, 3572),
            new TaxBand(87001, 180000, 0.37, 19822),
            new TaxBand(180001, Int32.MaxValue, 0.45, 54232)
        };

        public static TaxBand GetTaxBand(uint annualIncome)
        {
            return List.Find(b => b.TaxableIncomeLB <= annualIncome && b.TaxableIncomeUB >= annualIncome);
        }
    }

    public class TaxBand
    {
        public int TaxableIncomeLB { get; }
        public int TaxableIncomeUB { get; }
        public double VariableTax { get; }
        public int FlatTax { get; }

        public TaxBand(int taxableIncomeLB, int taxableIncomeUB, double variableTax, int flatTax)
        {
            this.TaxableIncomeLB = taxableIncomeLB;
            this.TaxableIncomeUB = taxableIncomeUB;
            this.VariableTax = variableTax;
            this.FlatTax = flatTax;
        }
    }
}
