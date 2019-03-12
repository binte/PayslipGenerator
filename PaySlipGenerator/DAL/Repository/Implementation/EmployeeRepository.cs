using PaySlipGenerator.DAL.Repository.Interfaces;
using PaySlipGenerator.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PaySlipGenerator.DAL.Context.Interfaces;

namespace PaySlipGenerator.DAL.Repository.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IContext Context;


        public EmployeeRepository(IContext context)
        {
            this.Context = context;
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
            Context.Add(entity);
        }

        public void AddRange(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }

        public void Persist()
        {
            Context.SetEmployees();
        }

        public void Remove(Employee entity)
        {
            Context.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Employee> entities)
        {
            throw new NotImplementedException();
        }
    }
}
