using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Audit.interfaces;

namespace Audit.Reports
{
    public abstract partial class ReportXls : IReport, IDisposable
    {
        protected string _filename;
        private int rn = 0, cn = 0;
        protected IWorkbook hssfwb;
        protected List<ICellStyle> styles = new List<ICellStyle>();
        protected List<IFont> fonts = new List<IFont>();

        protected IRow row;
        protected ICell cell;
        protected static ISheet sheet { get; set; } //Текущий лист

        public string filename { get { return _filename; } }
        public string rootPath { get; set; }

        public ReportXls()
        {

        }

        public virtual IWorkbook GetWorkBook()
        {
            return new XSSFWorkbook();

        }

        public void Init(string templateFileName = "")
        {
            _filename = templateFileName;
            hssfwb = GetWorkBook();
        }

        public abstract void CreateFonts();

        public abstract void CreateStyles();

        public void AddFont(IFont font)
        {
            fonts.Add(font);
        }

        public void AddStyle(ICellStyle style)
        {
            styles.Add(style);
        }

        public abstract void GenerateReport();

        public XSSFRow GetSheetRow(int rowIndex)
        {
            return (sheet.GetRow(rowIndex) != null) ? (XSSFRow)sheet.GetRow(rowIndex) : (XSSFRow)sheet.CreateRow(rowIndex);
        }

        public XSSFCell GetSheetCell(int cellIndex)
        {
            return (row.GetCell(cellIndex) != null) ? (XSSFCell)row.GetCell(cellIndex) : (XSSFCell)row.CreateCell(cellIndex);
        }

        public int CurrentRow { get {
                return rn - 1; 
        } }

        public void AddEmptyRow()
        {
            cn = 0;
            row = GetSheetRow(rn);
            cell = GetSheetCell(cn);
            cell.SetCellValue("");
            cn++;
            rn++;
        }

        public void SetCellValue(int cellIndex, string cellValue, ICellStyle style = null)
        {
            cell = GetSheetCell(cellIndex);
            cell.SetCellValue(cellValue);

            if (style != null)
                cell.CellStyle = style;
        }

        public void SetNumberCellValue(int cellIndex, int cellValue, ICellStyle style = null)
        {
            cell = GetSheetCell(cellIndex);
            cell.SetCellType(CellType.Numeric);
            cell.SetCellValue(cellValue);

            if (style != null)
                cell.CellStyle = style;
        }

        public void ActivateSheet(int cntSheets)
        {
            this.hssfwb.SetSelectedTab(cntSheets);
            this.hssfwb.SetActiveSheet(cntSheets);
        }

        public MemoryStream GetReportStream()
        {
            MemoryStream output = new MemoryStream();
            HSSFFormulaEvaluator.EvaluateAllFormulaCells(hssfwb);
            if (hssfwb is XSSFWorkbook)
                (hssfwb as XSSFWorkbook).Write(output, true);
            else
           if (hssfwb is HSSFWorkbook)
                (hssfwb as HSSFWorkbook).Write(output);

            return output;
        }

        //Сохраняем отчет и отправляем кому надо
        public string SaveReportOnDisc()
        {
            MemoryStream output = this.GetReportStream();
            string savePath = Path.Combine(rootPath, GetReportFileName());
            File.WriteAllBytes(savePath, output.ToArray());
            return savePath;
        }

        public void setBordersToMergedCells()
        {

            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                CellRangeAddress mergedRegions = sheet.GetMergedRegion(i);
                RegionUtil.SetBorderLeft((int)BorderStyle.Thin, mergedRegions, sheet, hssfwb);
                RegionUtil.SetBorderRight((int)BorderStyle.Thin, mergedRegions, sheet, hssfwb);
                RegionUtil.SetBorderTop((int)BorderStyle.Thin, mergedRegions, sheet, hssfwb);
                RegionUtil.SetBorderBottom((int)BorderStyle.Thin, mergedRegions, sheet, hssfwb);

            }
        }


        public virtual string GetReportFileName()
        {
            if (string.IsNullOrEmpty(_filename))
                return DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            else return _filename;
        }




        public void Dispose()
        {
            sheet = null;
            cell = null;
            row = null;
            hssfwb = null;
        }

    }
}
