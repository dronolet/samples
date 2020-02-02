using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;

namespace apitest.Interfaces
{
   
    public interface IAccountInterface
    {
        Task<IEnumerable<AcountHistoryModel>> GetAcountHistory(int account_id);

        Task TopUpAcount(int account_id, decimal amount);

        Task WithdrawAcount(int account_id, decimal amount);

        Task TransferAcount(int source_account_id, int destination_account_id, decimal amount);

    }
}
