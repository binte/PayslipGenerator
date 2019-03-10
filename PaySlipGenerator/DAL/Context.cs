using PaySlipGenerator.DAL.Models;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace PaySlipGenerator.DAL
{
    public class Context : FileContext
    {
        public Context(string originFilePath, string destinationFilePath, IFileSystem fileSystem, ILogger logger)
            : base(originFilePath, destinationFilePath, fileSystem, logger)
        {
        }


        private IList<Employee> _employees;
        public IList<Employee> Employees
        {
            get
            {
                if (_employees == null)
                {
                    _employees = this.ReadFile();
                }

                return _employees;
            }
        }

        public void SetEmployees()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                WriteStream(Employees, writer);     // 3) Write to stream
                WriteFile(stream);                  // 4) Write to file
            }
        }
    }
}
