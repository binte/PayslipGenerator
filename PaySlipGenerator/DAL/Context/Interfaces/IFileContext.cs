using PaySlipGenerator.DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PaySlipGenerator.DAL.Context.Interfaces
{
    public interface IFileContext
    {
        IList<Employee> ReadFile();
        void WriteStream(IList<Employee> employees, StreamWriter writer);
        void WriteFile(MemoryStream stream);
    }
}
