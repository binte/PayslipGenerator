using PaySlipGenerator.DAL.Models;
using PaySlipGenerator.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace PaySlipGenerator.DAL
{
    public class FileContext
    {
        public readonly string OriginFilePath;
        public readonly string DestinationFilePath;
        public readonly IFileSystem FileSystem;
        public readonly ILogger Logger;


        public FileContext() { }

        public FileContext(string originFilePath, string destinationFilePath, IFileSystem fileSystem, ILogger logger)
        {
            this.OriginFilePath = originFilePath;
            this.DestinationFilePath = destinationFilePath;
            this.FileSystem = fileSystem;
            this.Logger = logger;
        }


        public IList<Employee> ReadFile()
        {
            int n = 0;
            IList<Employee> employees = new List<Employee>();

            using (var reader = new StreamReader(FileSystem.File.OpenRead(this.OriginFilePath)))
            {
                while (!reader.EndOfStream)
                {
                    n++;
                    var line = reader.ReadLine();

                    try
                    {
                        Employee e = ParseEmployeeLine(line);
                        employees.Add(e);
                    }
                    catch (WrongParameterNumberException)
                    {
                        Logger.Error("Error in line {0} : {1}. || Line: {2}", n, "Wrong parameter number", line);
                    }
                    catch (AnnualIncomeFormatException)
                    {
                        Logger.Error("Error in line {0} : {1}. || Line: {2}", n, "Annual income in an incorrect format", line);
                    }
                    catch (SuperFormatException)
                    {
                        Logger.Error("Error in line {0} : {1}. || Line: {2}", n, "Super in an incorrect format", line);
                    }
                    catch (SuperOutOfBoundsException)
                    {
                        Logger.Error("Error in line {0} : {1}. || Line: {2}", n, "Super out of bounds [0-50]%", line);
                    }
                    catch (DateIntervalFormatException)
                    {
                        Logger.Error("Error in line {0} : {1}. || Line: {2}", n, "Date Interval in an incorrect format", line);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error in line {0} : {1}. || Line: {2}", n, ex.Message, line);
                    }
                }
            }

            Logger.Information("Finished reading file");

            return employees;
        }

        public void WriteStream(IList<Employee> employees, StreamWriter writer)
        {
            foreach (Employee e in employees)
            {
                writer.WriteLine(e.ToCsvString());
            }

            writer.Flush();
        }

        public void WriteFile(MemoryStream stream)
        {
            using (Stream file = File.Create(this.DestinationFilePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(file);
            }

            stream.Flush();
        }

        private Employee ParseEmployeeLine(string line)
        {
            string[] parameters = line.Split(','), dates;
            int super = 0;

            if (parameters.Length != 5)
            {
                throw new WrongParameterNumberException();
            }

            // No data validation is done to the names : too many cases to consider

            uint annualIncome = 0;
            try
            {
                annualIncome = UInt32.Parse(parameters[2]);
            }
            catch (Exception)
            {
                throw new AnnualIncomeFormatException();
            }

            try
            {
                string superStr = Regex.Match(parameters[3], "[0-9]*").Value;  // discard the % symbol
                super = Int32.Parse(superStr);
            }
            catch (Exception)
            {
                throw new SuperFormatException();
            }

            if (super < 0 || super > 50) { throw new SuperOutOfBoundsException(); }

            PaySlip p;
            try
            {
                dates = parameters[4].Split('-');  // separate the two dates
                p = new PaySlip(DateTime.Parse(dates[0] + " " + DateTime.Today.Year), DateTime.Parse(dates[1] + " " + DateTime.Today.Year));
            }
            catch (Exception)
            {
                throw new DateIntervalFormatException();
            }

            Employee e = new Employee(parameters[0], parameters[1], annualIncome, (double)super / 100, new List<PaySlip>() { p });
            p.Employee = e;
            return e;
        }
    }
}
