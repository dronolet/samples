using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Audit.models;

namespace Audit.interfaces
{
    public interface IReportsService
    {

        string  GetReportName(int report);
        Task<Stream> PeriodReport(PeriodReportTypeModel model);
        Task<Stream> OrdersReport(GetOrdersModel model, IEnumerable<RegistryListModel> data);
        Task<Stream> EmploeeReport(PeriodReportEmploeeModel model);
    }
}
