using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using Utilities.Logs;

namespace Models.Migration
{
    /// <summary>
    /// Класс, предназначенный для миграции данных из таблиц Excel
    /// </summary>
    public class FromExcel
    {
        /// <summary>
        /// Поле, предназначенное для логирования исполнения класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();

        /// <summary>
        /// Поле, предназначенное для логирования ошибок класса
        /// </summary>
        protected readonly ILogTools _loggerError = new ExceptionLog();

        /// <summary>
        /// Результат распарсивания Excel-таблицы
        /// </summary>
        public string[][] Output;

        /// <summary>
        /// Распарсивает таблицу (с ячейки A1) на первом листе Excel-файла
        /// </summary>
        /// <param name="fs">Поток Excel-файла</param>
        public FromExcel(FileStream fs)
        {
            try
            {
                _logger.WriteLn("Распарсивание Excel-файла");
                var tempFileName = Path.GetTempFileName();
                fs.Seek(0, SeekOrigin.Begin);
                fs.CopyTo(File.Open(tempFileName, FileMode.Open));
                using (var xlPackage = new ExcelPackage(new FileInfo(tempFileName)))
                {
                    var myWorksheet = xlPackage.Workbook.Worksheets[1];
                    var totalRows = myWorksheet.Dimension.End.Row;
                    var totalColumns = myWorksheet.Dimension.End.Column;
                    Output = new string[totalRows - 1][];
                    for (int i = 2; i <= totalRows; i++)
                    {
                        Output[i - 2] = myWorksheet.Cells[i, 1, i, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();
                    }
                }
                File.Delete(tempFileName);
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в конструкторе {typeof(FromExcel)}", ex);
            }
        }
    }
}
