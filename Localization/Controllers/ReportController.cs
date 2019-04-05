using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Reposity.Report;
using Localization.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DatabaseEntities.Reports;
using Utilities;

using OfficeOpenXml;
using Models.DatabaseEntities.DTO;
using OfficeOpenXml.Style;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public class reportParamsDTO
        {
            public Guid projectId { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public bool volumeCalcType { get; set; }
            public bool calcBasisType { get; set; }
            public User user { get; set; }
            public Locale locale { get; set; }
            public TypeOfServiceForSelectDTO workType { get; set; }
            public Guid initialFolderId { get; set; }
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
        [Authorize]
        [HttpPost]
        [Route("TranslatedWords")]
        public IEnumerable<TranslatedWordsReportRow> GetTranslatedWordsReport([FromBody] reportParamsDTO body)
        {
            TranslatedWordsReport translatedWords = new TranslatedWordsReport(Settings.GetStringDB());
            return translatedWords.GetRows(body.projectId, DateTime.Parse(body.start), DateTime.Parse(body.end),
                body.user == null ? Guid.Empty : body.user.id, body.locale == null ? Guid.Empty : body.locale.id, body.volumeCalcType, body.calcBasisType);
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
        [Authorize]
        [HttpPost]
        [Route("TranslatedWordsExcel")]
        public FileResult GetTranslatedWordsReportExcel([FromBody] reportParamsDTO body)
        {
            TranslatedWordsReport translatedWords = new TranslatedWordsReport(Settings.GetStringDB());
            List<TranslatedWordsReportRow> reportRows = translatedWords.GetRows(body.projectId, DateTime.Parse(body.start), DateTime.Parse(body.end),
                body.user == null ? Guid.Empty : body.user.id, body.locale == null ? Guid.Empty : body.locale.id, body.volumeCalcType, body.calcBasisType).ToList();

            FileResult res;

            using (ExcelPackage eP = new ExcelPackage())
            {
                eP.Workbook.Properties.Author = "Localization system";
                eP.Workbook.Properties.Title = "Отчет по количеству переведенных слов";
                eP.Workbook.Properties.Company = "Coderlink";

                var sheet = eP.Workbook.Worksheets.Add("Report");

                //Стили ячеек
                var styleBaseTableLeft = eP.Workbook.Styles.CreateNamedStyle("BaseTableLeft");
                styleBaseTableLeft.Style.Font.Name = "Arial";
                styleBaseTableLeft.Style.Font.Size = 10;
                styleBaseTableLeft.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                var styleBaseTableRight = eP.Workbook.Styles.CreateNamedStyle("BaseTableRight");
                styleBaseTableRight.Style.Font.Name = "Arial";
                styleBaseTableRight.Style.Font.Size = 10;
                styleBaseTableRight.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                var rowNum = 1;

                //Заголовок
                sheet.Cells[rowNum, 1].Value = "Отчет по работам";
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                sheet.Cells[rowNum, 1].Style.Font.Size = 16;
                sheet.Cells[rowNum, 1].Style.Font.Bold = true;
                rowNum += 2;

                //Отображение настроек фильтра
                sheet.Cells[rowNum, 1].Value = "Диапазон дат: " + body.start.Replace('.', '-') + " - " + body.end.Replace('.', '-');
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                rowNum++;
                sheet.Cells[rowNum, 1].Value = "Пользователь: " + (body.user != null ? body.user.Name_text : "Все пользователи");
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                rowNum++;
                sheet.Cells[rowNum, 1].Value = "Язык: " + (body.locale != null ? body.locale.name_text : "Все языки");
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                rowNum++;
                sheet.Cells[rowNum, 1].Value = "Вид работ: " + (body.workType != null ? body.workType.name_text : "Все");
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                rowNum++;
                sheet.Cells[rowNum, 1].Value = "Подсчет объема по: " + (body.volumeCalcType ? "знакам с пробелами" : "словам");
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                rowNum++;
                sheet.Cells[rowNum, 1].Value = "Основа: " + (body.calcBasisType ? "язык перевода" : "исходный язык");
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                rowNum += 2;


                // шапка
                sheet.Cells[rowNum, 1].Value = "Имя пользователя";
                sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                sheet.Cells[rowNum, 1].Style.Font.Bold = true;
                sheet.Cells[rowNum, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[rowNum, 2].Value = "Языки";
                sheet.Cells[rowNum, 2].StyleName = styleBaseTableLeft.Name;
                sheet.Cells[rowNum, 2].Style.Font.Bold = true;
                sheet.Cells[rowNum, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[rowNum, 3].Value = "Вид работ";
                sheet.Cells[rowNum, 3].StyleName = styleBaseTableRight.Name;
                sheet.Cells[rowNum, 3].Style.Font.Bold = true;
                sheet.Cells[rowNum, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                sheet.Cells[rowNum, 4].Value = "Переведено";
                sheet.Cells[rowNum, 4].StyleName = styleBaseTableRight.Name;
                sheet.Cells[rowNum, 4].Style.Font.Bold = true;
                sheet.Cells[rowNum, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                foreach (var row in reportRows)
                {
                    rowNum++;
                    sheet.Cells[rowNum, 1].Value = row.name_text;
                    sheet.Cells[rowNum, 1].StyleName = styleBaseTableLeft.Name;
                    sheet.Cells[rowNum, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowNum, 2].Value = row.language;
                    sheet.Cells[rowNum, 2].StyleName = styleBaseTableLeft.Name;
                    sheet.Cells[rowNum, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowNum, 3].Value = row.work_Type;
                    sheet.Cells[rowNum, 3].StyleName = styleBaseTableRight.Name;
                    sheet.Cells[rowNum, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet.Cells[rowNum, 4].Value = row.translations;
                    sheet.Cells[rowNum, 4].StyleName = styleBaseTableRight.Name;
                    sheet.Cells[rowNum, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

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
