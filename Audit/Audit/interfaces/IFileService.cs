using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Audit.models;
using System.Threading.Tasks;

namespace Audit.interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<FileListModel>> GetOrderFiles(int orderid);
        Task AddFile(int orderid);
        Task DeleteFile(int id);
        Task sendMail(SendMailModel model);
        Task DeleteOrderFiles(int orderid);
        string GetFileName(int id, out string realname);
        void DeleteOrderDir(int orderid);
        void sendDocMail(SendMailModel model, int? headId = null, string contractorName = null);
    }
}
