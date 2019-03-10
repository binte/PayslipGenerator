using PaySlipGenerator.ApL.Services.Interfaces;
using Serilog;
using System;

namespace PaySlipGeneratorRunner
{
    public class App
    {
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly ILogger Logger;

        public App(ITaxCalculatorService taxCalculatorService, ILogger logger)
        {
            _taxCalculatorService = taxCalculatorService;
            this.Logger = logger;
        }

        public void Run()
        {
            Logger.Information("Payslip generation task started");

            try
            {
                _taxCalculatorService.GeneratePayslips();     // 1 & 2) Read file, and generate Payslips

                Logger.Information("Finished generating payslips");
            }
            catch (Exception ex)
            {
                Logger.Error("Payslips could not be generated: {0}", ex.Message);
            }

            try
            {
                _taxCalculatorService.StoreData();            // 3 & 4) Write to Stream, and then to File

                Logger.Information("Finished writing to file");
            }
            catch (Exception ex)
            {
                Logger.Error("Data could not be stored: {0}", ex.Message);
            }

            Logger.Information("Payslip generation task finished");

            Console.ReadKey();
        }
    }
}
