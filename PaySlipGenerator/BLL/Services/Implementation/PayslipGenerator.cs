using PaySlipGenerator.BLL.Services.Interfaces;
using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.Exceptions;
using Serilog;
using System;

/**
 * Domain Service
 */
namespace PaySlipGenerator.BLL.Services.Implementation
{
    public class PayslipGenerator : IPayslipGenerator
    {
        public PayslipGenerator() { }

        public void GeneratePayslips(Employee emp, ITaxCalculator taxCalculator, ILogger logger)
        {
            foreach (PaySlip p in emp.Payslips)
            {
                if (!p.Generated)
                {
                    try
                    {
                        GeneratePayslip(taxCalculator, p, emp.AnnualIncome, emp.SuperRate);
                    }
                    catch (NegativeNumberException ex)
                    {
                        logger.Error(string.Format("Negative Number error while generating payslip for {0} {1} : {2}", emp.FirstName, emp.LastName, ex.Message));
                    }
                    catch (PayslipGenerationException ex)
                    {
                        logger.Error(string.Format("Error generating payslip for {0} {1} : {2}", emp.FirstName, emp.LastName, ex.Message));
                    }
                }
            }
        }

        public void GeneratePayslip(ITaxCalculator calculator, PaySlip payslip, uint annualIncome, double superRate)
        {
            try
            {
                payslip.GrossIncome = calculator.GrossIncome(annualIncome);
                payslip.IncomeTax = calculator.IncomeTax(annualIncome);
                payslip.NetIncome = payslip.GrossIncome - payslip.IncomeTax;
                payslip.Super = calculator.Super(annualIncome, superRate);
                payslip.Generated = true;
            }
            catch (NegativeNumberException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PayslipGenerationException(ex.Message, ex);
            }
        }
    }
}
