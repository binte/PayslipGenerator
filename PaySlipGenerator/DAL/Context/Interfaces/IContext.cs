using PaySlipGenerator.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaySlipGenerator.DAL.Context.Interfaces
{
    public interface IContext
    {
        IList<Employee> Employees { get; }

        void SetEmployees();
        void Add(Employee e);
        void Remove(Employee entity);
    }
}
