﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DAL.Reposity.PostgreSqlRepository;
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
        private readonly IFilesRepository _filesRepository;

        public ReportController(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        [HttpGet]
        [Route("TranslatedWords")]
        public IEnumerable<TranslatedWordsReportRow> GetTranslatedWordsReport(
            [FromQuery] string start,
            [FromQuery] string end,
            [FromQuery] string volumeCalcType,
            [FromQuery] string calcBasisType,
            [FromQuery] int? userId,
            [FromQuery] int? localeId,
            [FromQuery] string workType,
            [FromQuery] int? initialFolderId)
        {
            TranslatedWordsReport TranslatedWords = new TranslatedWordsReport();
            return TranslatedWords.GetRows(DateTime.Parse(start), DateTime.Parse(end));
        }

        [HttpGet]
        [Route("TranslatedWordsExcel")]
        public FileResult GetTranslatedWordsReportExcel(
            [FromQuery] string start,
            [FromQuery] string end,
            [FromQuery] string volumeCalcType,
            [FromQuery] string calcBasisType,
            [FromQuery] int? userId,
            [FromQuery] int? localeId,
            [FromQuery] string workType,
            [FromQuery] int? initialFolderId)
        {
            TranslatedWordsReport TranslatedWords = new TranslatedWordsReport();
            List<TranslatedWordsReportRow> reportRows = TranslatedWords.GetRows(DateTime.Parse(start), DateTime.Parse(end)).ToList();
            byte[] bin;

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

                bin = eP. GetAsByteArray();

                string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string file_name = "Report.xlsx";
                res = File(bin, file_type, file_name);       
            }

            return res;
        }
    }
}
