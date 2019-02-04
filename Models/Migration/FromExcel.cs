using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using System.Linq;
using Utilities.Logs;
using Models.DatabaseEntities;

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
        /// Конструктор по умолчанию, не содержащий кода
        /// </summary>
        public FromExcel()
        {

        }

        /// <summary>
        /// Распарсивает таблицу <see cref="Locale"/>'ей (с ячейки A1) на первом листе Excel-файла и записывает извлеченные сущности в БД
        /// </summary>
        /// <param name="fs">Поток Excel-файла</param>
        public void UploadLocalesFromExcel(System.IO.FileStream fs)
        {
            try
            {
                _logger.WriteLn(string.Format("Распарсивание Excel-файла и запись извлеченных сущностей {0} в БД", nameof(Locale)));
                var tempFileName = System.IO.Path.GetTempFileName();
                fs.Seek(0, System.IO.SeekOrigin.Begin);
                fs.CopyTo(System.IO.File.Open(tempFileName, System.IO.FileMode.Open));
                using (var xlPackage = new ExcelPackage(new System.IO.FileInfo(tempFileName)))
                {
                    var myWorksheet = xlPackage.Workbook.Worksheets[1];
                    var totalRows = myWorksheet.Dimension.End.Row;
                    var totalColumns = myWorksheet.Dimension.End.Column;
                    for (int i = 2; i <= totalRows; i++)
                    {
                        string[] input = myWorksheet.Cells[i, 1, i, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();
                        var loc = new Locale(input[0], string.Empty, input[1], null, input[2]);
                        //can't use DAL-project to upload Locale because of the circular dependency problem
                    }
                }
                System.IO.File.Delete(tempFileName);
            }
            catch (Exception ex)
            {
                _loggerError.WriteLn($"Ошибка в конструкторе {typeof(FromExcel)}", ex);
            }
        }
    }
}
