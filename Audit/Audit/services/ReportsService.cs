using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using audit.db;
using Audit.interfaces;
using Audit.models;
using Audit.Reports;

namespace Audit.services
{
    public class ReportsService : IReportsService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private DBCommonContext _dbcontext;

        public ReportsService(DBCommonContext dbcontext, IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
            _dbcontext = dbcontext;
        }


        public  string GetReportName(int report) {
            if (report == 1)
                return "ОтчетПоСотрудникам";
            else
            if (report == 2)
             return "ОтчетПоПодрядчикам";
            else
            if (report == 3)
                return "ОтчетПоСотруднику";
            else
            if (report == 4)
                return "ОтчетПоОбъектам";
            else return "Отчет";
        }


        public Task<Stream> OrdersReport(GetOrdersModel model, IEnumerable<RegistryListModel> data)
        {
            return Task<Stream>.Run(() =>
            {

                using (ReportOrders report = new ReportOrders(_dbcontext, model, data))
                {
                    report.Init();
                    report.GenerateReport();
                    return report.GetReportStream() as Stream;
                }
            });
        }

        public Task<Stream> PeriodReport(PeriodReportTypeModel model) {
            return Task<Stream>.Run(() =>
            {

                if (model.ReportId == 1)
                {
                    using (ReportAllEmploee report = new ReportAllEmploee(_dbcontext, model.DFrom, model.DTo))
                    {
                        report.Init();
                        report.GenerateReport();
                        return report.GetReportStream() as Stream;
                    }
                }
                else 
                if (model.ReportId == 2)
                {
                    using (ReportContructors report = new ReportContructors(_dbcontext, model.DFrom, model.DTo))
                    {
                        report.Init();
                        report.GenerateReport();
                        return report.GetReportStream() as Stream;
                    }
                }
                else
                if (model.ReportId == 4)
                {
                    using (ReportAllObjects report = new ReportAllObjects(_dbcontext, model.DFrom, model.DTo))
                    {
                        report.Init();
                        report.GenerateReport();
                        return report.GetReportStream() as Stream;
                    }
                }
                return null;
            });
        }

        public Task<Stream> EmploeeReport(PeriodReportEmploeeModel model)
        {
            return Task<Stream>.Run(() =>
            {
                using (ReportByEmployee report = new ReportByEmployee(_dbcontext, model.DFrom, model.DTo, model.EmploeeId))
                {
                    report.Init();
                    report.GenerateReport();
                    return report.GetReportStream() as Stream;
                }
            });
        }

    }
}
