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
     public class AccountService: IAccountInterface
    {
        private const string ERROR_NEGATIVE_BALANCE = "Отрицательный баланс";
        private const string ERROR_NEGATIVE_AMOUNT = "Сумма должна быть > 0";
        private const string ERROR_ACCOUNT_NOTFOUND = "Счет не найден.";
        private const string ERROR_ACCOUNT_SOURCE_NOTFOUND = "Счет источника не найден.";
        private const string ERROR_ACCOUNT_DESTINATION_NOTFOUND = "Счет назначения не найден.";
        private const string ERROR_INPUT_ACCOUNTS = "Счет источника и назначения одинаковы";

        private DBCommonContext _dbcontext;
        
        public AccountService(DBCommonContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        void AddAccount(Account accuont, decimal amount)
        {
            if (accuont.balance + amount < 0)
                throw new BadLogicException(ERROR_NEGATIVE_BALANCE);
            accuont.balance = accuont.balance + amount;

            _dbcontext.AccountHistorys.Add(new AccountHistory()
            {
               AccountId = accuont.id,
               Amount = amount
            });
            
        }

        public Task<IEnumerable<AcountHistoryModel>> GetAcountHistory(int account_id)
        {
            return Task.Run(() =>
            {
                var account_info = _dbcontext.Accounts.Where(o => o.id == account_id).Include(o => o.AccountAccountHistorys).AsNoTracking().FirstOrDefault();
                if  (account_info == null)
                    throw new NotFindException(ERROR_ACCOUNT_NOTFOUND);
                return account_info.AccountAccountHistorys.Select(s => new AcountHistoryModel() { Id = s.Id, Account_number = account_info.account_number, Amount = s.Amount, ChangedAt = s.ChangedAt }).OrderByDescending(o =>  o.ChangedAt).AsEnumerable();
            });
        }

        public Task TopUpAcount(int account_id, decimal amount) {
            return Task.Run(() =>
            {
                if (amount <= 0)
                    throw new BadParametersException(ERROR_NEGATIVE_AMOUNT);

                Account account = _dbcontext.Accounts.FirstOrDefault(o => o.id == account_id);
                if (account == null)
                    throw new NotFindException(ERROR_ACCOUNT_NOTFOUND);

                 AddAccount(account, amount);
                _dbcontext.SaveChanges();
             
            });
        }

        public Task WithdrawAcount(int account_id, decimal amount) {
            return Task.Run(() =>
            {
                if (amount <= 0)
                    throw new BadParametersException(ERROR_NEGATIVE_AMOUNT);

                Account account = _dbcontext.Accounts.FirstOrDefault(o => o.id == account_id);
                if (account == null)
                    throw new NotFindException(ERROR_ACCOUNT_NOTFOUND);

                AddAccount(account, -amount);
                _dbcontext.SaveChanges();

            
            });
        }

        public Task TransferAcount(int source_account_id, int destination_account_id, decimal amount) {
            return Task.Run(() =>
            {

                if (amount <= 0)
                    throw new BadParametersException(ERROR_NEGATIVE_AMOUNT);

                if (source_account_id == destination_account_id)
                  throw new BadParametersException(ERROR_INPUT_ACCOUNTS);

                Account source_account = _dbcontext.Accounts.FirstOrDefault(o => o.id == source_account_id);
                if (source_account == null)
                    throw new NotFindException(ERROR_ACCOUNT_SOURCE_NOTFOUND);

                Account destination_account = _dbcontext.Accounts.FirstOrDefault(o => o.id == destination_account_id);
                if (destination_account == null)
                    throw new NotFindException(ERROR_ACCOUNT_DESTINATION_NOTFOUND);

                AddAccount(source_account, -amount);
                AddAccount(destination_account, amount);
                _dbcontext.SaveChanges();
            });
        }



    }
}
