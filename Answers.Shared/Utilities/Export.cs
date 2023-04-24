using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Answers.Shared.Utilities
{
    public class Export
    {
        public static byte[] ExportExcel<T>(IEnumerable<T> data, out string fileNameSuggested)
        {
            var stream = new MemoryStream();

            byte[] fileContents;

            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(data, true);

                Color colFromHex = ColorTranslator.FromHtml("#1F4763");
                Color white = ColorTranslator.FromHtml("#FFFFFF");
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(colFromHex);
                workSheet.Row(1).Style.Font.Color.SetColor(white);
                workSheet.Row(1).Style.Font.Bold = true;

                fileContents = package.GetAsByteArray();
            }

            stream.Position = 0;
            fileNameSuggested = $"{DateTime.Now.ToString("yyyyMMdd")}.xlsx";

            return fileContents;
        }
    }
}
