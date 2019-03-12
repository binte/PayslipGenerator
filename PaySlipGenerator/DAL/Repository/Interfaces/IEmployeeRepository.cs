using PaySlipGenerator.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PaySlipGenerator.DAL.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee Get(int id);
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> Find(Expression<Func<Employee, bool>> predicate);

        void Add(Employee entity);
        void AddRange(IEnumerable<Employee> entities);
        void Persist();
        void Remove(Employee entity);
        void RemoveRange(IEnumerable<Employee> entities);
    }
}
