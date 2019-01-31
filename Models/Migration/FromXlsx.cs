using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;

namespace Models.Migration
{
    class FromXlsx
    {
        public string[][] LocalesInfo;

        public FromXlsx(byte[] fileData)
        {
            try
            {
                string tempFileName = Path.GetTempFileName();
                File.WriteAllBytes(tempFileName, fileData);
                using (var xlPackage = new ExcelPackage(new FileInfo(tempFileName)))
                {
                    var myWorksheet = xlPackage.Workbook.Worksheets[1];
                    var totalRows = myWorksheet.Dimension.End.Row;
                    var totalColumns = myWorksheet.Dimension.End.Column;
                    this.LocalesInfo = new string[totalRows - 1][];
                    for (int i = 2; i <= totalRows; i++)
                    {
                        LocalesInfo[i - 2] = myWorksheet.Cells[i, 1, i, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();
                    }
                }
                File.Delete(tempFileName);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
