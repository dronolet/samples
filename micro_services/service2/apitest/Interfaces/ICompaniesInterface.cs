using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;
using apitest.Objects;

namespace apitest.Interfaces
{
   
    public interface ICompaniesInterface
    {
        Task UpdateCompanies();
        string GetCompany(int id);

        Task<IEnumerable<Company>> GetCompanies();

    }
}
