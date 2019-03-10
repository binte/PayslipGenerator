namespace PaySlipGenerator.BLL.Services.Interfaces
{
    public interface ITaxCalculator
    {
        uint GrossIncome(uint annualIncome);
        uint IncomeTax(uint annualIncome);
        uint NetIncome(uint annualIncome);
        uint Super(uint annualIncome, double superRate);
        TaxBand GetTaxBand(uint annualIncome);
    }
}
