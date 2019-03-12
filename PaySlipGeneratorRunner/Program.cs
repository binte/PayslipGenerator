using Microsoft.Extensions.DependencyInjection;
using PaySlipGenerator.ApL.Services.Implementation;
using PaySlipGenerator.ApL.Services.Interfaces;
using PaySlipGenerator.BLL.Services.Implementation;
using PaySlipGenerator.BLL.Services.Interfaces;
using PaySlipGenerator.DAL.Context.Implementation;
using PaySlipGenerator.DAL.Context.Interfaces;
using PaySlipGenerator.DAL.Repository.Implementation;
using PaySlipGenerator.DAL.Repository.Interfaces;
using Serilog;
using System;
using System.IO.Abstractions;

namespace PaySlipGeneratorRunner
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Log.Logger.Error("Wrong input : The program expects an input filename and an output filename");
                Console.ReadKey();
                return 1;
            }

            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, args[0], args[1]);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<App>().Run();

            return 0;
        }

        private static void ConfigureServices(IServiceCollection services, string originFilePath, string destinationFilePath)
        {
            // configure logging
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs-main.txt")
            .WriteTo.Console()
            .CreateLogger();

            // add logging
            services.AddSingleton<ILogger>(Log.Logger);

            // add context
            services.AddScoped<IFileContext>(_ => new FileContext(originFilePath, destinationFilePath, new FileSystem(), Log.Logger));
            services.AddScoped<IContext, Context>();

            // add repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // add domain services
            services.AddTransient<ITaxCalculator, TaxCalculator>();
            services.AddTransient<IPayslipGenerator, PayslipGenerator>();

            // add application services
            services.AddTransient<ITaxCalculatorService, TaxCalculatorService>();

            // add app
            services.AddTransient<App>();
        }
    }
}
