using System;
using AutoMapper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;

using Audit.interfaces;
using audit.db;
using Audit.models;
using Audit.exceptions;
using Audit.objects;

namespace Audit.services
{
    public class FileService : IFileService
    {
        private DBCommonContext _dbcontext;
        private IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        private IAuthService _authService;
        private string  _filePath;
        private ISendMailService _sendMailService;

        public FileService(
             DBCommonContext dbcontext, 
             IHttpContextAccessor httpContextAccessor, 
             IAuthService AuthService, 
             IConfiguration configuration,
             ISendMailService sendMailService
            ) {
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _filePath = _configuration.GetValue<string>("Repository");
            _authService = AuthService;
            _sendMailService = sendMailService;
        }

        public Task<IEnumerable<FileListModel>> GetOrderFiles(int clientid)
        {
            return Task<IEnumerable<FileListModel>>.Run(() =>
            {
                

                var files = _dbcontext.OrderFiles.Where(o => o.OrderId == clientid).AsNoTracking().ToList();

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrderFile, FileListModel>()).CreateMapper();
                return mapper.Map<List<OrderFile>, List<FileListModel>>(files).AsEnumerable<FileListModel>();
            });
        }

        public string GetFileName(int id, out string realname) {

            realname = "";
            var file = _dbcontext.OrderFiles.Where(o => o.Id == id).AsNoTracking().FirstOrDefault();
            if (file == null)
                throw new NotFoundException();
            realname = file.Filename;
            return Path.Combine(_filePath,file.OrderId.ToString(), file.RepFileName);
        }

        public void sendDocMail(SendMailModel model, int? headId = null, string contractorName = null)
        {
            var order = _dbcontext.Orders.Where(o => o.Id == model.OrderId).AsNoTracking().FirstOrDefault();
                if (order == null)
                    throw new NotFoundException();

                var emails = new List<string>();
                headId = headId??order.Head;
                contractorName = ((contractorName?? order.Contractor)??"").ToUpper().Trim(); 
                if (headId.HasValue)
                    emails = _dbcontext.HeadStroyUsers.Where(o => o.id == headId.Value && o.Email != null && o.Email.Length > 0).Select(o => o.Email).AsNoTracking().ToList<string>();

                var Enemails = _dbcontext.MainEnginiers.Where(o => o.Email != null && o.Email.Length > 0).Select(o => o.Email).AsNoTracking().ToList<string>();
                if (Enemails.Count > 0)
                   emails.AddRange(Enemails);

                var contractor = _dbcontext.Kontragents.Where(o => o.name.ToUpper().Trim() == contractorName).AsNoTracking().FirstOrDefault();

                List<FileAttachment> files = null;

            if (model.Ids != null)
            {
                files = _dbcontext.OrderFiles.Where(f => model.Ids.Contains(f.Id)).Select(f => new FileAttachment()
                {
                    FilePath = Path.Combine(_filePath, order.Id.ToString(), f.RepFileName),
                    FileName = f.Filename
                }).ToList<FileAttachment>();
            }
            else {
                files = _dbcontext.OrderFiles.Where(f => f.OrderId == model.OrderId).Select(f => new FileAttachment()
                {
                    FilePath = Path.Combine(_filePath, order.Id.ToString(), f.RepFileName),
                    FileName = f.Filename
                }).ToList<FileAttachment>();
            }

            if (contractor != null && (contractor.email ?? "").Trim().Length > 0)
                emails.Add(contractor.email);

            if (files.Count > 0 && emails.Count > 0)
            {
                
                string mess = _sendMailService.SendMail("no-reply@l1-stroy.ru", string.Join(", ", emails.ToArray()), "Предписания", "Отправитель: " + _authService.GetUserInfoModel().FullName, files);
                if (mess.Length != 0)
                  throw new SendEmailException("Ошибка отправки.");
            }
        }

        public Task sendMail(SendMailModel model) {
            return Task.Run(() =>
            {
                sendDocMail(model);
            });
        }

        public Task AddFile(int orderid) {
            return Task.Run(() =>
            {
                var order = _dbcontext.Orders.Where(o => o.Id == orderid).AsNoTracking().FirstOrDefault();
                if (order == null)
                    throw new NotFoundException();

                var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
                Stream stream = file.OpenReadStream();

                if (stream.Length > 0)
                {
                    string filePathFull = Path.Combine(_filePath, orderid.ToString()); //Path.GetFileName(file.FileName)
                    if (!Directory.Exists(filePathFull))
                         Directory.CreateDirectory(filePathFull);

                    string filename = Guid.NewGuid().ToString(); // + Path.GetExtension(file.FileName);

                    filePathFull = Path.Combine(filePathFull, filename);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        File.WriteAllBytes(filePathFull, ms.ToArray());
                    }

                    try
                    {
                        OrderFile orderFile = new OrderFile()
                        {
                            Id = 0,
                            Filename = Path.GetFileName(file.FileName),
                            OrderId = order.Id,
                            RepFileName = filename,
                            userid = _authService.CurrentUserId()
                    };

                        _dbcontext.Update(orderFile);
                        _dbcontext.SaveChanges();
                    }
                    catch (Exception ex) {
                        if (File.Exists(filePathFull))
                            File.Delete(filePathFull);
                        GC.Collect();
                        throw new Exception("Ошибка загрузки файла");
                    }
                }

            });
        }

        public Task DeleteFile(int id) {
            return Task.Run(() =>
            {

                var file = _dbcontext.OrderFiles.Where(o => o.Id == id).FirstOrDefault();

                if (file == null)
                    throw new NotFoundException();

                string filePathFull = Path.Combine(_filePath,file.OrderId.ToString(), file.RepFileName);

                if (File.Exists(filePathFull))
                {
                    File.Delete(filePathFull);
                    GC.Collect();
                }

                _dbcontext.OrderFiles.Remove(file);
                _dbcontext.SaveChanges();

               

            });
        }

        public void DeleteOrderDir(int orderid)
        {
            string dirName = Path.Combine(_filePath, orderid.ToString());
            if (Directory.Exists(dirName))
                Directory.Delete(dirName);
        }

        public Task DeleteOrderFiles(int orderid)
        {
            return Task.Run(() =>
            {
                try
                {
                    List<Task> delTasks = new List<Task>();
                    var files = _dbcontext.OrderFiles.Where(o => o.OrderId == orderid).AsNoTracking().ToList();
                    if (files != null && files.Count > 0)
                        foreach (var file in files)
                        {
                            if (file == null)
                                throw new NotFoundException();

                            string filePathFull = Path.Combine(_filePath, file.OrderId.ToString(), file.RepFileName);

                            if (File.Exists(filePathFull))
                              File.Delete(filePathFull);
                               
                            _dbcontext.OrderFiles.Remove(file);
                            _dbcontext.SaveChanges();

                        }
                    DeleteOrderDir(orderid);
                }
                finally {
                    GC.Collect();
                }
            });
            //return Task.WhenAll(delTasks.ToArray());
        }
    }
}
