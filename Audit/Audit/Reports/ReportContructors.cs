using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using audit.db;
using System;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Audit.Reports
{
    public class ReportContructors : ReportXls
    {
        private DBCommonContext _dbcontext;
        private DateTime? _dfrom;
        private DateTime? _dto;

        public ReportContructors(DBCommonContext dbcontext, DateTime? dfrom, DateTime? dto) : base()
        {
            _dbcontext = dbcontext;
            _dfrom = dfrom;
            _dto = dto;
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
            commonStyle.WrapText = true;
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

            // клиент
            commonStyle = hssfwb.CreateCellStyle();
            commonStyle.VerticalAlignment = VerticalAlignment.Center;
            commonStyle.Alignment = HorizontalAlignment.Left;
            commonStyle.WrapText = true;
            commonStyle.FillForegroundColor = IndexedColors.Rose.Index;
            commonStyle.FillPattern = FillPattern.SolidForeground;
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
            commonStyle.FillForegroundColor = IndexedColors.LightGreen.Index;
            commonStyle.FillPattern = FillPattern.SolidForeground;
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
            commonStyle.FillForegroundColor = IndexedColors.PaleBlue.Index;
            commonStyle.FillPattern = FillPattern.SolidForeground;
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
            commonStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            commonStyle.FillPattern = FillPattern.SolidForeground;
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
            sheet.SetColumnWidth(1, 9000);
            sheet.SetColumnWidth(2, 3000);
            sheet.SetColumnWidth(3, 3000);
            sheet.SetColumnWidth(4, 4000);
            sheet.SetColumnWidth(5, 3000);
            sheet.SetColumnWidth(6, 4000);
            sheet.SetColumnWidth(7, 3000);
            sheet.SetColumnWidth(8, 4000);
            sheet.SetColumnWidth(9, 3000);

            AddEmptyRow();
            SetCellValue(1, "Дата отчета: " + DateTime.Now.ToString("dd.MM.yyyy"), styles[0]);
            AddEmptyRow();
            AddEmptyRow();
            SetCellValue(1, "Отчет по подрядчикам", styles[0]);
            AddEmptyRow();
            SetCellValue(1, string.Format("(формируется на период с {0} по {1})", _dfrom.Value.ToString("dd.MM.yyyy"), _dto.Value.ToString("dd.MM.yyyy")), styles[0]);
            AddEmptyRow();
            AddEmptyRow();
            sheet.AddMergedRegion(new CellRangeAddress(CurrentRow, CurrentRow, 2,5));
            sheet.AddMergedRegion(new CellRangeAddress(CurrentRow, CurrentRow, 6, 7));
            sheet.AddMergedRegion(new CellRangeAddress(CurrentRow, CurrentRow, 8, 9));
            SetCellValue(2, "Из них", styles[1]);
            SetCellValue(6, "В работе", styles[1]);
            SetCellValue(8, "Просрочен", styles[1]);
            setBordersToMergedCells();
            AddEmptyRow();
            SetCellValue(1, "ФИО", styles[1]);
            SetCellValue(2, "Всего выходов за период", styles[1]);
            SetCellValue(3, "Замечания", styles[1]);
            SetCellValue(4, "Предписания", styles[1]);
            SetCellValue(5, "Без замечаний", styles[1]);

            SetCellValue(6, "Предписания", styles[1]);
            SetCellValue(7, "Замечания", styles[1]);

            SetCellValue(8, "Предписания", styles[1]);
            SetCellValue(9, "Замечания", styles[1]);

        }

        public void FillData()
        {
            var list = _dbcontext.ReportSatistics.FromSql("GetReportSatistic @report, @dfrom, @dto",
                  new SqlParameter("@report", 2),
                  new SqlParameter("@dfrom", _dfrom),
                  new SqlParameter("@dto", _dto)
                ).OrderByDescending(o => o.outputs).AsNoTracking().ToList();

            foreach (var data in list)
            {
                AddEmptyRow();
                SetCellValue(1, Convert.ToString(data.name), styles[3]);
                SetNumberCellValue(2, data.outputs, styles[4]);
                SetNumberCellValue(3, data.remarks, styles[4]);
                SetNumberCellValue(4, data.notifs, styles[4]);
                SetNumberCellValue(5, data.noremarks, styles[4]);

                SetNumberCellValue(6, data.wnotifs, styles[5]);
                SetNumberCellValue(7, data.wremarks, styles[5]);

                SetNumberCellValue(8, data.onotifs, styles[6]);
                SetNumberCellValue(9, data.oremarks, styles[6]);
            }
            AddEmptyRow();
            SetCellValue(1, "Итого:", styles[1]);
            SetNumberCellValue(2, list.Sum(o => o.outputs), styles[4]);
            SetNumberCellValue(3, list.Sum(o => o.remarks), styles[4]);
            SetNumberCellValue(4, list.Sum(o => o.notifs), styles[4]);
            SetNumberCellValue(5, list.Sum(o => o.noremarks), styles[4]);

            SetNumberCellValue(6, list.Sum(o => o.wnotifs), styles[5]);
            SetNumberCellValue(7, list.Sum(o => o.wremarks), styles[5]);

            SetNumberCellValue(8, list.Sum(o => o.onotifs), styles[6]);
            SetNumberCellValue(9, list.Sum(o => o.oremarks), styles[6]);
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
