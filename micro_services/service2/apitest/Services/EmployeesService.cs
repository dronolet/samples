using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using apitest.Models;
using apitest.Modules;
using apitest.Objects;
using apitest.Exceptions;
using apitest.Classes;
using apitest.Interfaces;

namespace apitest.Services
{

    /// <summary>
    /// Сервис для работы со счетами
    /// </summary>
     public class EmployeesService: IEmployeeInterface
    {

        
        
        private DBCommonContext _dbcontext;
        private ICompaniesInterface _companiesSrvice;

        public EmployeesService(DBCommonContext dbcontext, ICompaniesInterface companiesSrvice)
        {
            _dbcontext = dbcontext;
            _companiesSrvice = companiesSrvice;
        }

        public Task<IEnumerable<EmployeesListModel>> GetEmployees()
        {
            ;
            return Task.Run(() =>
            {
                var companies = _companiesSrvice.GetCompanies().Result;
                return _dbcontext.Employees.AsNoTracking()
                 .Join(companies,
                    p => p.CompanyId,
                    c => c.Id,
                    (p, c) => new EmployeesListModel()
                    {
                        Id = p.Id,
                        Address = p.Address,
                        BirthDay = p.BirthDay,
                        Company = c.Name,
                        Fio = p.Fio,
                        Number = p.Number,
                        CompanyId = p.CompanyId
                    }
                 ).AsEnumerable();
            });
        }

        public Task UpdateEmployees(int? id, EmployeesDataModel model)
        {
            return Task.Run(() =>
            {

                
                Employee employee = null;
                if (id.HasValue)
                {
                    employee = _dbcontext.Employees.FirstOrDefault(o => o.Id == id);
                    if (employee == null)
                        throw new NotFindException("Сотрудник не найден");
                }
                else employee = new Employee();

                if (_companiesSrvice.GetCompany(model.CompanyId) == null)
                    throw new NotFindException("Компания не найдена");

                employee.Fio = model.Fio;
                employee.Number = model.Number;
                employee.CompanyId = model.CompanyId;
                employee.Address = model.Address;
                employee.BirthDay = model.BirthDay;
                _dbcontext.Employees.Update(employee);
               _dbcontext.SaveChanges();
            });
        }

        public Task DeleteEmployees(int id)
        {
            return Task.Run(() =>
            {
                var employee = _dbcontext.Employees.FirstOrDefault(o => o.Id == id);
                if (employee == null)
                    throw new NotFindException("Сотрудник не найден");

                _dbcontext.Employees.Remove(employee);
                _dbcontext.SaveChanges();
            });
        }



    }
}
