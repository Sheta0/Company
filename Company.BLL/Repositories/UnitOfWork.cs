using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IEmployeeRepository EmployeeRepository { get; } // NULL

        public IDepartmentRepository DepartmentRepository { get; } // NULL

        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            EmployeeRepository = new EmployeeRepository(_context);
            DepartmentRepository = new DepartmentRepository(_context);
        }

        public int Complete()
            => _context.SaveChanges();


        public void Dispose()
            => _context.Dispose();
    }
}
