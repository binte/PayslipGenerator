using PaySlipGenerator.ApL.Services.Interfaces;
using PaySlipGenerator.BLL.Services.Interfaces;
using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.DAL.Repository.Interfaces;
using Serilog;

namespace PaySlipGenerator.ApL.Services.Implementation
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly IEmployeeRepository Repository;
        private readonly ITaxCalculator Calculator;
        private readonly IPayslipGenerator Generator;
        private readonly ILogger Logger;


        public TaxCalculatorService(IEmployeeRepository employeeRepository, ITaxCalculator calculator, IPayslipGenerator generator, ILogger logger)
        {
            Repository = employeeRepository;
            Calculator = calculator;
            Generator = generator;
            Logger = logger;
        }


        public void GeneratePayslips()
        {
            foreach( Employee emp in Repository.GetAll() )
            {
                Generator.GeneratePayslips(emp, Calculator, Logger);
            }
        }

        public void StoreData()
        {
            Repository.Persist();
        }
    }
}
