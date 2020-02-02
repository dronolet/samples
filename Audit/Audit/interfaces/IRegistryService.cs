using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using audit.db;
using Audit.models;
using Audit.objects;

namespace Audit.interfaces
{
    public interface IRegistryService
    {
        Task<IEnumerable<RegistryListModel>> GetRegistryData(GetOrdersModel model);
        Task UpdateOrder(int? id, RegistryModel model);
        Task DeleteOrder(int id);
        Task<RegistryModel> GetOrder(int id);
        Task<IEnumerable<BObject>> GetBObjects();
        Task<IEnumerable<KontragentModel>> GetKontragents(string path);
        Task<IEnumerable<HeadStroyUserModel>> GetHeadStroyUsers();
    }
}
