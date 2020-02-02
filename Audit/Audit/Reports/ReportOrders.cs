using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using audit.db;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Audit.models;
using Microsoft.EntityFrameworkCore;


namespace Audit.Reports
{
    public class ReportOrders : ReportXls
    {
        private DBCommonContext _dbcontext;
        private DateTime? _dfrom;
        private DateTime? _dto;
        private int _overdue;
        private IEnumerable<RegistryListModel> _data;

        public ReportOrders(DBCommonContext dbcontext, GetOrdersModel model, IEnumerable<RegistryListModel> data) : base()
        {
            _dbcontext = dbcontext;
            _dfrom = model.dfrom;
            _dto = model.dto;
            _overdue = model.overdue;
            _data = data;
        }


        public override void CreateFonts()
        {
            var font = hssfwb.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            AddFont(font);

            font = hssfwb.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            AddFont(font);
        }

        public override void CreateStyles()
        {
            ICellStyle commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Left;
            commonStyle.WrapText = false;
            commonStyle.SetFont(fonts[0]);
            AddStyle(commonStyle);

            commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Center;
            commonStyle.WrapText = true;
            commonStyle.BorderTop = BorderStyle.Thin;
            commonStyle.BorderBottom = BorderStyle.Thin;
            commonStyle.BorderLeft = BorderStyle.Thin;
            commonStyle.BorderRight = BorderStyle.Thin;
            commonStyle.SetFont(fonts[1]);
            AddStyle(commonStyle);


            // номер
            commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Right;
            commonStyle.WrapText = true;
            commonStyle.BorderTop = BorderStyle.Thin;
            commonStyle.BorderBottom = BorderStyle.Thin;
            commonStyle.BorderLeft = BorderStyle.Thin;
            commonStyle.BorderRight = BorderStyle.Thin;
            commonStyle.SetFont(fonts[0]);
            AddStyle(commonStyle);

            ///////
            commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Left;
            commonStyle.WrapText = true;
            commonStyle.BorderTop = BorderStyle.Thin;
            commonStyle.BorderBottom = BorderStyle.Thin;
            commonStyle.BorderLeft = BorderStyle.Thin;
            commonStyle.BorderRight = BorderStyle.Thin;
            commonStyle.SetFont(fonts[0]);
            AddStyle(commonStyle);


            commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Center;
            commonStyle.WrapText = true;
            commonStyle.BorderTop = BorderStyle.Thin;
            commonStyle.BorderBottom = BorderStyle.Thin;
            commonStyle.BorderLeft = BorderStyle.Thin;
            commonStyle.BorderRight = BorderStyle.Thin;
            commonStyle.SetFont(fonts[0]);
            AddStyle(commonStyle);


            commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Center;
            commonStyle.WrapText = true;

            commonStyle.BorderTop = BorderStyle.Thin;
            commonStyle.BorderBottom = BorderStyle.Thin;
            commonStyle.BorderLeft = BorderStyle.Thin;
            commonStyle.BorderRight = BorderStyle.Thin;
            commonStyle.SetFont(fonts[0]);
            AddStyle(commonStyle);

            commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Center;
            commonStyle.WrapText = true;
            commonStyle.BorderTop = BorderStyle.Thin;
            commonStyle.BorderBottom = BorderStyle.Thin;
            commonStyle.BorderLeft = BorderStyle.Thin;
            commonStyle.BorderRight = BorderStyle.Thin;
            commonStyle.SetFont(fonts[0]);
            AddStyle(commonStyle);

        }

        public void SetTitle()
        {
            sheet.SetColumnWidth(0, 3000);
            sheet.SetColumnWidth(1, 4000);
            sheet.SetColumnWidth(2, 4000);
            sheet.SetColumnWidth(3, 5000);
            sheet.SetColumnWidth(4, 8000);
            sheet.SetColumnWidth(5, 8000);
            sheet.SetColumnWidth(6, 8000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 4000);
            sheet.SetColumnWidth(9, 3000);
            sheet.SetColumnWidth(10, 8000);
            sheet.SetColumnWidth(11, 8000);

            AddEmptyRow();
            SetCellValue(1, "Дата отчета: " + DateTime.Now.ToString("dd.MM.yyyy"), styles[0]);
            AddEmptyRow();
            AddEmptyRow();
            SetCellValue(1, "Замечания ОНОР", styles[0]);
            AddEmptyRow();            
            SetCellValue(1, string.Format("(формируется на период с {0} по {1})", _dfrom.Value.ToString("dd.MM.yyyy"), _dto.Value.ToString("dd.MM.yyyy")), styles[0]);
            AddEmptyRow();
            SetCellValue(1, string.Format("cроки: {0}",(_overdue == 1?"Да": "Нет")), styles[0]);
            AddEmptyRow();
            AddEmptyRow();
            AddEmptyRow();
            SetCellValue(1,  "Дата", styles[1]);
            SetCellValue(2,  "Временной интервал", styles[1]);
            SetCellValue(3,  "Объект", styles[1]);
            SetCellValue(4,  "Место", styles[1]);
            SetCellValue(5,  "Подрядчик", styles[1]);

            SetCellValue(6,  "Сотрудник ОНОР", styles[1]);
            SetCellValue(7,  "Результат", styles[1]);

            SetCellValue(8,  "Дата устранения", styles[1]);
            SetCellValue(9, "Дата устр. факт.", styles[1]);
            SetCellValue(10, "Вид работ", styles[1]);
            SetCellValue(11, "Примечание", styles[1]);

        }

        public void FillData()
        {
            foreach (var data in _data)
            {
                AddEmptyRow();
                SetCellValue(1, data.DBegin.ToString("dd.MM.yyyy"), styles[4]);
                SetCellValue(2, data.TimeInterval, styles[4]);
                SetCellValue(3, data.BuildObjectName, styles[4]);
                SetCellValue(4, data.Location, styles[4]);
                SetCellValue(5, data.Contractor, styles[4]);

                SetCellValue(6, data.Employee, styles[4]);
                SetCellValue(7, data.ResultCaption, styles[4]);

                SetCellValue(8, data.Repare.ToString("dd.MM.yyyy"), styles[4]);
                SetCellValue(9, (data.RepareFakt.HasValue? data.RepareFakt.Value.ToString("dd.MM.yyyy"):"-"), styles[4]);
                SetCellValue(10, data.WorkType, styles[3]);
                SetCellValue(11, data.Remark, styles[4]);
            }
        }

        public override void GenerateReport()
        {

            sheet = (XSSFSheet)hssfwb.CreateSheet("Лист1");
            CreateFonts();
            CreateStyles();
            SetTitle();
            FillData();
        }


    }
}