using PaySlipGenerator.DAL.Models;
using Serilog;

namespace PaySlipGenerator.BLL.Services.Interfaces
{
    public interface IPayslipGenerator
    {
        void GeneratePayslips(Employee emp, ITaxCalculator taxCalculator, ILogger logger);
        void GeneratePayslip(ITaxCalculator calculator, PaySlip payslip, uint annualIncome, double superRate);
    }
}
