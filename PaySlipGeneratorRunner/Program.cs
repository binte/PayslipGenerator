using PaySlipGenerator;
using Serilog;
using System;
using System.Collections.Generic;

namespace PaySlipGeneratorRunner
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs-main.txt")
                .WriteTo.Console()
                .CreateLogger();

            if (args.Length == 0)
            {
                Log.Logger.Error("File with Employee Data not specified");
                Console.ReadKey();
                return 1;
            }

            Log.Logger.Information("Payslip generation started");
            IO.SetLogger(Log.Logger);
            List<Employee> employees = IO.ReadEmployeeData(args[0]);

            Log.Logger.Information("Payslip generation finished");

            Console.ReadKey();

            return 0;
        }
    }
}
