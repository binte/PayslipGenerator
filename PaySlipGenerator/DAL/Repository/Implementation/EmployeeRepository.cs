using PaySlipGenerator.DAL.Repository.Interfaces;
using PaySlipGenerator.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PaySlipGenerator.DAL.Repository.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        protected readonly Context Context;


        public EmployeeRepository(Context context)
        {
            Context = context;
        }


        public virtual Employee Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetAll()
        {
            return Context.Employees.ToList();
        }

        public IEnumerable<Employee> Find(Expression<Func<Employee, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(Employee entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public void AddAll()
        {
            Context.SetEmployees();
        }

        public void Remove(Employee entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }
    }
}
