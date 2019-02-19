using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PaySlipGenerator
{
    public class IO
    {
        public static List<Employee> ReadEmployeeData(string path)
        {
            List<Employee> employees = new List<Employee>();

            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    Employee e = ParseEmployeeLine(line);
                    employees.Add(e);
                }
            }

            return employees;
        }

        public static Employee ParseEmployeeLine(string line)
        {
            string[] parameters = line.Split(','), dates;
            string super = Regex.Match(parameters[3], "[0-9]*").Value;  // discard the % symbol

            dates = parameters[4].Split('-');  // separate the two dates
            PaySlip p = new PaySlip(DateTime.Parse(dates[0] + " " + DateTime.Today.Year), DateTime.Parse(dates[1] + " " + DateTime.Today.Year));
            Employee e = new Employee(parameters[0], parameters[1], Double.Parse(parameters[2]), Int32.Parse(super), new List<PaySlip>() { p });
            p.Employee = e;
            return e;
        }
    }
}
