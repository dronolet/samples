using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Audit.interfaces;
using audit.db;
using Audit.models;
using Audit.objects;
using Audit.exceptions;

namespace Audit.services
{
    public class RegistryService: IRegistryService
    {
        private const int DaysToBlock = 3;

        private IAuthService _authService;
        private DBCommonContext _dbcontext;
        private IFileService _fileService;
        private DBDictinaryContext _dictcontext;
        private IHolyDaysService _holyDaysService;
        

        public RegistryService(
            IHolyDaysService holyDaysService, 
            DBCommonContext dbcontext, 
            IHttpContextAccessor httpContextAccessor, 
            IAuthService AuthService, 
            IFileService FileService
            ) {
            _dbcontext = dbcontext;
            _authService = AuthService;
            _fileService = FileService;
            _holyDaysService = holyDaysService;
        }

        private string getShortName(string name) {
            string[] nameParts = name.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            return nameParts[0] + (nameParts.Length > 1? " " + nameParts[1].ToCharArray()[0] + ". " +  (nameParts.Length > 2?" " + nameParts[2].ToCharArray()[0] + ". ":"") :"") ;
        }

        public Task<IEnumerable<KontragentModel>> GetKontragents(string path) {
            return Task.Run(() =>
            {
                var Kontragents = _dbcontext.Kontragents.Where(o => o.name.ToLower() .Contains(path.ToLower())).AsNoTracking().ToList();
                Kontragents.ForEach(o => o.name = o.name.Trim());
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Kontragent, KontragentModel>()).CreateMapper();
                var model = mapper.Map<List<Kontragent>, List<KontragentModel>>(Kontragents).OrderByDescending(o => o.Name).ToList();
                return model.AsEnumerable<KontragentModel>();
            });
        }

        public Task<IEnumerable<BObject>> GetBObjects()
        {
            return Task<IEnumerable<BObject>>.Run(() =>
            {
                var objects = _dbcontext.BObjects.ToList().AsEnumerable();
                return objects;
            });
        }

        public Task<IEnumerable<HeadStroyUserModel>> GetHeadStroyUsers()
        {
            return Task<IEnumerable<HeadStroyUserModel>>.Run(() =>
            {
                var heads = _dbcontext.HeadStroyUsers.ToList();
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HeadStroyUser, HeadStroyUserModel>()).CreateMapper();
                var model = mapper.Map<List<HeadStroyUser>, List<HeadStroyUserModel>>(heads).AsEnumerable();
                return model;
            });
        }

        

        public Task<IEnumerable<RegistryListModel>> GetRegistryData(GetOrdersModel model) {

            bool isoverdue = model.overdue == 1;
            return Task<IEnumerable<RegistryListModel>>.Run(() =>
            {
                int currentUserId = _authService.CurrentUserId();
                var orders = _dbcontext.Orders.Where(o => 
                (o.UserId == currentUserId || _authService.isFullViewer())
                && (model.dfrom.HasValue?o.DBegin.Date >= model.dfrom:true)
                && (model.dto.HasValue ? o.DBegin.Date <= model.dto: true)
                ).AsNoTracking().ToList();

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Order, RegistryListModel>()).CreateMapper();
                List<RegistryListModel> outmodel = mapper.Map<List<Order>, List<RegistryListModel>>(orders).OrderBy(o => o.DBegin).ToList<RegistryListModel>();
                outmodel.ForEach(
                    
                    o => {
                           o.Employee = getShortName(_dbcontext.SprBudjetUsers.Where(s => s.Id == o.UserId).FirstOrDefault()?.FullName);
                           o.BuildObjectName = _dbcontext.BObjects.Where(s => s.Id == o.BuildObject).FirstOrDefault()?.Name ?? "";
                           o.ResultCaption = (o.Result == 1 ? "Без замечания" : (o.Result == 2 ? "Замечание" : "Предписание"));
                           o.HeadId = (_authService.isHeadOfUser(o.UserId.Value) ? _authService.CurrentUserId() : 0);
                           o.CanEdit = (o.DBegin.Date <= DateTime.Now.Date && DateTime.Now.Date < (o.BlockDate.HasValue ? o.BlockDate.Value : o.DBegin.Date.AddDays(DaysToBlock)) && o.UserId == currentUserId ? 1 : (_authService.isHeadOfUser(o.UserId.Value) ? 1 : 0));
                           o.isOverTime = (o.Result != 1 && o.Repare < DateTime.Now.Date && (!o.RepareFakt.HasValue || o.RepareFakt.HasValue && o.RepareFakt > o.Repare) ? 1 : 0);
                        } 
                
                );

                if (isoverdue)
                    return outmodel.OrderByDescending(o => o.isOverTime).ThenByDescending(o => o.DBegin).AsEnumerable<RegistryListModel>();
                return outmodel.OrderByDescending(o => o.DBegin).AsEnumerable<RegistryListModel>();
            });
        }

        public Task<RegistryModel> GetOrder(int id)
        {
            return Task.Run(() =>
            {
                var order = _dbcontext.Orders.FirstOrDefault(o => o.Id == id);
                if (order == null)
                    throw new NotFoundException();

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Order, RegistryModel>()).CreateMapper();
                var model = mapper.Map<Order, RegistryModel>(order);
                SprBudjetUser user = _dbcontext.SprBudjetUsers.Where(o => o.Id == model.UserId).FirstOrDefault();
                if (user != null)
                    model.Employee = user.FullName;


                model.CanEdit = (order.DBegin.Date <= DateTime.Now.Date && DateTime.Now.Date < (order.BlockDate.HasValue ? order.BlockDate.Value : order.DBegin.Date.AddDays(DaysToBlock)) && order.UserId == _authService.CurrentUserId() ? 1 : (_authService.isHeadOfUser(order.UserId) ? 1 : 0));
                return model;
            });
        }


        public Task UpdateOrder(int? id, RegistryModel model) {
            return Task.Run(() =>
            {
                Order order = null;
                int oldResult = 0;

                if (id != null)
                 {
                    order = _dbcontext.Orders.AsNoTracking().FirstOrDefault(o => o.Id == model.Id);
                    if (order == null)
                         throw new NotFoundException();

                    if (!_authService.isHeadOfUser(order.UserId))
                    {
                        if (order.BlockDate <= DateTime.Now)
                            throw new Exception($"Нельзя редактировать записи давностью больше {DaysToBlock + 1} дней");
                    }

                    oldResult = order.Result;
                }

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RegistryModel, Order>()).CreateMapper();
                Order modelOrder = mapper.Map<RegistryModel, Order>(model);
                if (id == null)
                {
                    order = modelOrder;
                    order.UserId = _authService.CurrentUserId();
                }
               

                order.DBegin = modelOrder.DBegin.ToLocalTime();
                order.DEnd = modelOrder.DEnd.ToLocalTime();
                order.BuildObject = modelOrder.BuildObject;
                order.Contractor = modelOrder.Contractor;
                order.Location = modelOrder.Location;
                order.Remark = modelOrder.Remark;
                order.Result = modelOrder.Result;
                order.Head = (modelOrder.Result == 3?modelOrder.Head:null);
                order.Repare = modelOrder.Repare.ToLocalTime().Date;
                order.RepareFakt = (modelOrder.RepareFakt.HasValue? modelOrder.RepareFakt.Value.ToLocalTime().Date: modelOrder.RepareFakt);
                
                order.Worktype = modelOrder.Worktype;

                if (order.BlockDate == null)
                {
                    
                    var holyWorkDays = _holyDaysService.HolyWorkDays(modelOrder.DBegin).ToList();
                    var holyDays = _holyDaysService.HolyDays(modelOrder.DBegin).ToList();

                    DateTime InspectDate = modelOrder.DBegin.Date.Date;
                    int WorkDaysCounter = 0;
                    while (WorkDaysCounter <= DaysToBlock)
                    {
                        InspectDate = InspectDate.AddDays(1);
                        if (
                               (InspectDate.DayOfWeek != DayOfWeek.Saturday && InspectDate.DayOfWeek != DayOfWeek.Sunday && !holyDays.Contains(InspectDate))
                               ||
                               ((InspectDate.DayOfWeek == DayOfWeek.Saturday || InspectDate.DayOfWeek == DayOfWeek.Sunday) && holyWorkDays.Contains(InspectDate))
                           )
                            WorkDaysCounter++;
                    }
                    order.BlockDate = InspectDate;
                }

                _dbcontext.Update(order);
                _dbcontext.SaveChanges();

                if (id != null && modelOrder.Result == 3 && oldResult != modelOrder.Result)
                  _fileService.sendDocMail(new SendMailModel() { OrderId = order.Id }, order.Head, order.Contractor);

                
            });
        }

        public Task DeleteOrder(int id)
        {
            return Task.Run(() =>
            {
                var order = _dbcontext.Orders.FirstOrDefault(o => o.Id == id);

                if (order == null)
                    throw new NotFoundException();


                if ((DateTime.Now - order.DBegin).TotalDays > DaysToBlock)
                    throw new Exception($"Нельзя удалять записи давностью больше {DaysToBlock + 1} дней");

                Task delTask  = _fileService.DeleteOrderFiles(id);
                delTask.Wait();
                
               
                _dbcontext.Orders.Remove(order);
                _dbcontext.SaveChanges();

            });
        }
    }
}
