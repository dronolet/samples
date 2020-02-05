using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;
using apitest.Objects;

namespace apitest.Interfaces
{
   
    public interface ICompanyInterface
    {
        Task<IEnumerable<Company>> GetCompanies();
        Task UpdateCompany(int? id, CompanyDataModel model);
        Task DeleteCompany(int id);

    }
}
