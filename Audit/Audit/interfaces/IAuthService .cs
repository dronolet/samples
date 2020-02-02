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
    public interface IAuthService
    {
        Task Sign(string login, string password);
        Task SignOut();
        Task <UserInfoModel> GetUserInfo();
        SprBudjetUser getUserById(int id);
        SprBudjetUser CurrentUserById();
        int CurrentUserId();
        List<SprBudjetUser> GetUsers();
        Task Remember(string email);
        Task RememberAll();
        bool isHeadOfUser(int userId);
        bool isFullViewer();
        Task<IEnumerable<EmploeeModel>> GetEmployeers();

        UserInfoModel GetUserInfoModel();
    }
}
