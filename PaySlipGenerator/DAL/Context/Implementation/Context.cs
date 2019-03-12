using PaySlipGenerator.DAL.Context.Interfaces;
using PaySlipGenerator.DAL.Models;
using Serilog;
using System.Collections.Generic;
using System.IO;

namespace PaySlipGenerator.DAL.Context.Implementation
{
    public class Context : IContext
    {
        private readonly IFileContext FileContext;
        private IList<Employee> _employees;
        public IList<Employee> Employees
        {
            get
            {
                if (_employees == null)
                {
                    _employees = FileContext.ReadFile();
                }

                return _employees;
            }
        }

        public Context(IFileContext fileContext)
        {
            this.FileContext = fileContext;
        }


        public void SetEmployees()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                FileContext.WriteStream(Employees, writer);     // 3) Write to stream
                FileContext.WriteFile(stream);                  // 4) Write to file
            }
        }

        public void Add(Employee e)  // Adds to a cache, will not yet be persisted
        {
            this.Employees.Add(e);
        }

        public void Remove(Employee entity)  // Removes entity from cache
        {
            this.Employees.Remove(entity);
        }
    }
}
