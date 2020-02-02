using System.IO;

namespace Audit.interfaces
{
    public interface IReport
    {
        void Init(string templateFileName = "");
        int CurrentRow { get; }
        void GenerateReport();
        MemoryStream GetReportStream();
        void setBordersToMergedCells();
        string SaveReportOnDisc();
        string GetReportFileName();
        void CreateFonts();
        void CreateStyles();
    }
}
