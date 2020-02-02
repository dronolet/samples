using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;

using Audit.interfaces;
using audit.db;
using Audit.models;
using Audit.exceptions;
using Audit.objects;
using Audit.Helper;

namespace Audit.services
{
    public class AuthService : IAuthService
    {
        private DBCommonContext _dbcontext;
        private IHttpContextAccessor _httpContextAccessor;

        public AuthService(DBCommonContext dbcontext, IHttpContextAccessor httpContextAccessor) {
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;
        }


        public  SprBudjetUser getUserById(int id)
        {
             return GetUsers().Where(o => o.Id == id).FirstOrDefault();
        }

     


        public List<SprBudjetUser> GetUsers()
        {
            return _dbcontext.SprBudjetUsers.Where(u => (u.Password ?? "").Length > 0).AsNoTracking().Distinct().ToList();
        }

        public Task<IEnumerable<EmploeeModel>> GetEmployeers()
        {
            return Task<IEnumerable<EmploeeModel>>.Run(() =>
            {
                var managers = _dbcontext.HeadManagers.AsNoTracking().Select( o => o.id).ToArray();
                var users = _dbcontext.SprBudjetUsers.Where(u => (u.Password ?? "").Length > 0)
                .Where(o => !managers.Contains(o.Id)).Distinct().OrderBy(o => o.FullName).AsNoTracking().AsEnumerable();

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SprBudjetUser, EmploeeModel>()).CreateMapper();
                return mapper.Map< IEnumerable<SprBudjetUser>, IEnumerable<EmploeeModel>>(users);
            });
        }

        public int CurrentUserId()
        {
            var clame = _httpContextAccessor.HttpContext.User.Claims.Where(o => o.Type == ClaimsIdentity.DefaultNameClaimType).FirstOrDefault();
            return Convert.ToInt32(clame.Value);
        }

        public bool isHeadOfUser(int userId)
        {
            try
            {
                return _dbcontext.HeadManagers.AsNoTracking().Any(o => (o.id == CurrentUserId() && o.isAdmin == 1));
            }
            catch {
                return false;
            }
        }

        public bool isFullViewer()
        {
            try
            {
                return _dbcontext.HeadManagers.AsNoTracking().Any(o => o.id == CurrentUserId());
            }
            catch 
            {
                return false;
            }
        }


        public SprBudjetUser CurrentUserById()
        {
            return  getUserById(CurrentUserId());
        }

        public Task Sign(string login, string password) {

            var user = GetUsers().Where(o => o.UserLogin == login && o.Password == password).FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim> {
               new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
            };

                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
            else throw new NotFoundException();
        }

        public Task Remember(string email)
        {
            return Task.Run(() =>
            {
                var user = GetUsers().Where(o => o.email == email).FirstOrDefault();
                if (user != null)
                {
                    SendMailer.SendMail("no-reply@l1-stroy.ru", user.email, "Пароль на ресурс Замечания ОНОР ( http://web:9037/)",
                      string.Format("Логин: {0}, Пароль: {1}", user.UserLogin, user.Password));

                }
                else throw new NotFoundException();
            });
        }


        public Task RememberAll()
        {
            return Task.Run(() =>
            {
                foreach (var user in GetUsers())
                    try
                    {
                        SendMailer.SendMail("no-reply@l1-stroy.ru", user.email, "Пароль на ресурс http://web:9037/ (Сотркдники ОНОР)",
                               string.Format("Логин: {0}, Пароль: {1}", user.UserLogin, user.Password));
                    }
                    catch
                    {

                    }
            });
        }


        public Task SignOut() {
            return _httpContextAccessor.HttpContext.SignOutAsync();
        }


        public UserInfoModel GetUserInfoModel()
        {
            var clame = _httpContextAccessor.HttpContext.User.Claims.Where(o => o.Type == ClaimsIdentity.DefaultNameClaimType).FirstOrDefault();
            SprBudjetUser user = getUserById(Convert.ToInt32(clame.Value));

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SprBudjetUser, UserInfoModel>()).CreateMapper();
            var model = mapper.Map<SprBudjetUser, UserInfoModel>(user);
            model.isHead = (isHeadOfUser(0) ? 1 : 0);
            model.isRevisor = (isFullViewer() ? 1 : 0);
            return model;
        }

        public Task<UserInfoModel> GetUserInfo() {
            return Task<UserInfoModel>.Run(() =>
            {
                return GetUserInfoModel();
            });
        }

    }
}
