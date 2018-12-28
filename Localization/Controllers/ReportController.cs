﻿using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Reposity.Report;
using Microsoft.AspNetCore.Mvc;
using Models.Reports;
using OfficeOpenXml;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
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
        public IEnumerable<TranslatedWordsReportRow> GetTranslatedWordsReport(
            [FromBody] int projectId,
            [FromBody] string start,
            [FromBody] string end,
            [FromBody] bool volumeCalcType,
            [FromBody] bool calcBasisType,
            [FromBody] int userId,
            [FromBody] int localeId,
            [FromBody] string workType,
            [FromBody] int initialFolderId)
        {
            TranslatedWordsReport translatedWords = new TranslatedWordsReport();
            return translatedWords.GetRows(projectId, DateTime.Parse(start), DateTime.Parse(end), userId, localeId, volumeCalcType, calcBasisType);
        }

        /// <summary>
        /// Получаем файл содержащий строки отчета в формате Excel
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
        /// <returns>Файл Report.xlsx</returns>
        [HttpPost]
        [Route("TranslatedWordsExcel")]
        public FileResult GetTranslatedWordsReportExcel(
            [FromBody] int projectId,
            [FromBody] string start,
            [FromBody] string end,
            [FromBody] bool volumeCalcType,
            [FromBody] bool calcBasisType,
            [FromBody] int userId,
            [FromBody] int localeId,
            [FromBody] string workType,
            [FromBody] int initialFolderId)
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

                var bin = eP. GetAsByteArray();

                string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string file_name = "Report.xlsx";
                res = File(bin, file_type, file_name);       
            }

            return res;
        }
    }
}
