using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using apitest.Objects;
using apitest.Interfaces;
using service.Helpers;

namespace apitest.Services
{

    /// <summary>
    /// Сервис для работы со счетами
    /// </summary>
     public class CompanyService : ICompaniesInterface
    {
        private string _companyhost;
        private static object updateLocker = new object();
        private static HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(2) };

        // справочник компаний
        private static List<Company> companyList = new List<Company>();
        


        public CompanyService(string companyhost)
        {
            _companyhost = companyhost;
            UpdateCompanies();
           
        }

        public string GetCompany(int id) {
            lock (updateLocker)
            {
                return CompanyService.companyList.FirstOrDefault(o => o.Id == id)?.Name;
            }
        }

        public Task<IEnumerable<Company>> GetCompanies()
        {
            return Task.Run(() =>
            {
                lock (updateLocker)
                {
                    return CompanyService.companyList.AsEnumerable();
                }
                
            });
        }

        public async Task UpdateCompanies()
        {
            try
            {
                if (!string.IsNullOrEmpty(_companyhost))
                {
                    var responseResult = JsonHelper.Deserialize<JSONResult>(await client.GetStringAsync(_companyhost));
                    lock (updateLocker)
                    {
                        CompanyService.companyList = responseResult.result.ToList();
                    }
                }
            }
            catch {

            }
        }


    }
}
