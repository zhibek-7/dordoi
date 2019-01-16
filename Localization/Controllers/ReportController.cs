using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Reposity.Report;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities.Reports;

using OfficeOpenXml;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public class reportParamsDTO
        {
            public int projectId { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public bool volumeCalcType { get; set; }
            public bool calcBasisType { get; set; }
            public int userId { get; set; }
            public int localeId { get; set; }
            public string workType { get; set; }
            public int initialFolderId { get; set; }
        }


        /// <summary>
        /// Получаем отчет
        /// </summary>
        /// <param name="projectId">ID проекта локализации</param>
        /// <param name="start">Начало выборки</param>
        /// <param name="end">Конец выборки</param>
        /// <param name="volumeCalcType">Считать по символам с пробелами? Если false то по словам</param>
        /// <param name="calcBasisType">>Считать по переводам? Если false то по исходным строкам</param>
        /// <param name="userId">ID пользователя</param>
        /// <param name="localeId">ID языка</param>
        /// <param name="workType">Тип работы - перевод и редактура</param>
        /// <param name="initialFolderId">ID начальной папки</param>
        /// <returns>Строки отчета</returns>
        [HttpPost]
        [Route("TranslatedWords")]
        public IEnumerable<TranslatedWordsReportRow> GetTranslatedWordsReport([FromBody] reportParamsDTO _params)
        {
            TranslatedWordsReport translatedWords = new TranslatedWordsReport();
            return translatedWords.GetRows(_params.projectId, DateTime.Parse(_params.start), DateTime.Parse(_params.end), 
                _params.userId, _params.localeId, _params.volumeCalcType, _params.calcBasisType);
        }

        /// <summary>
        /// Получаем файл содержащий строки отчета в формате Excel
        /// </summary>
        /// <param name="projectId">ID проекта локализации</param>
        /// <param name="start">Начало выборки</param>
        /// <param name="end">Конец выборки</param>\\
        /// <param name="volumeCalcType">Считать по символам с пробелами? Если false то по словам</param>
        /// <param name="calcBasisType">>Считать по переводам? Если false то по исходным строкам</param>
        /// <param name="userId">ID пользователя</param>
        /// <param name="localeId">ID языка</param>
        /// <param name="workType">Тип работы - перевод и редактура</param>
        /// <param name="initialFolderId">ID начальной папки</param>
        /// <returns>Файл Report.xlsx</returns>
        [HttpPost]
        [Route("TranslatedWordsExcel")]
        public FileResult GetTranslatedWordsReportExcel(
            int projectId,
            string start,
            string end,
            bool volumeCalcType,
            bool calcBasisType,
            int userId,
            int localeId,
            string workType,
            int initialFolderId)
        {
            TranslatedWordsReport translatedWords = new TranslatedWordsReport();
            List<TranslatedWordsReportRow> reportRows = translatedWords.GetRows(projectId, DateTime.Parse(start), DateTime.Parse(end), userId, localeId, volumeCalcType, calcBasisType).ToList();

            FileResult res;

            using (ExcelPackage eP = new ExcelPackage())
            {
                eP.Workbook.Properties.Author = "Localization system";
                eP.Workbook.Properties.Title = "Отчет по количеству переведенных слов";
                eP.Workbook.Properties.Company = "Coderlink";

                var sheet = eP.Workbook.Worksheets.Add("Report");

                var rowNum = 1;

                // шапка
                sheet.Cells[rowNum, 1].Value = "Пользователь";
                sheet.Cells[rowNum, 2].Value = "Языки";
                sheet.Cells[rowNum, 3].Value = "Вид работ";
                sheet.Cells[rowNum, 4].Value = "Переведено";

                foreach (var row in reportRows)
                {
                    rowNum++;
                    sheet.Cells[rowNum, 1].Value = row.Name;
                    sheet.Cells[rowNum, 2].Value = row.Language;
                    sheet.Cells[rowNum, 3].Value = row.workType;
                    sheet.Cells[rowNum, 4].Value = row.Translations;

                    // указываем что в этой ячейке число
                    //sheet.Cells[row, 3].Style.Numberformat.Format = @"#,##0.00_ ;\-#,##0.00_ ;0.00_ ;";
                }

                var bin = eP.GetAsByteArray();

                string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string file_name = "Report.xlsx";
                res = File(bin, file_type, file_name);
            }

            return res;
        }
    }
}
