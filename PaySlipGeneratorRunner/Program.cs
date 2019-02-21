using PaySlipGenerator;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

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

            if (args.Length != 2)
            {
                Log.Logger.Error("Wrong input : The program expects an input filename and an output filename");
                Console.ReadKey();
                return 1;
            }

            Log.Logger.Information("Payslip generation started");
            IO.SetLogger(Log.Logger);
            List<Employee> employees = IO.ReadEmployeeData(args[0]);        // 1) Read file

            foreach (Employee e in employees) { e.GeneratePayslips(); }     // 2) Generate payslips

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                IO.WriteToStream(employees, writer);                        // 3) Write to stream
                IO.WriteToFile(args[1], stream);                            // 4) Write to file
            }

            Log.Logger.Information("Payslip generation finished");

            Console.ReadKey();

            return 0;
        }
    }
}
