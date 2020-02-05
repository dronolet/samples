using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;
using apitest.Objects;

namespace apitest.Interfaces
{
   
    public interface IEmployeeInterface
    {
        Task<IEnumerable<EmployeesListModel>> GetEmployees();
        Task UpdateEmployees(int? id, EmployeesDataModel model);
        Task DeleteEmployees(int id);

    }
}
