﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        [HttpGet]
        [Route("TranslatedWords:{dateFrom}:{dateTo}")]
        public IEnumerable<TranslatedWordsReportRow> GetTranslatedWordsReport()
        {
            TranslatedWordsReport TranslatedWords = new TranslatedWordsReport();
            List<TranslatedWordsReportRow> reportRows = TranslatedWords.Get().ToList();
            return reportRows;
        }

        [HttpGet]
        [Route("TranslatedWordsExcel:{dateFrom}:{dateTo}")]
        public FileResult GetTranslatedWordsReportExcel()
        {
            TranslatedWordsReport TranslatedWords = new TranslatedWordsReport();
            List<TranslatedWordsReportRow> reportRows = TranslatedWords.Get().ToList();
            byte[] bin;
            HttpResponseMessage result = null;

            FileResult res;

            using (ExcelPackage eP = new ExcelPackage())
            {
                eP.Workbook.Properties.Author = "Localization system";
                eP.Workbook.Properties.Title = "Отчет по количеству переведенных слов";
                eP.Workbook.Properties.Company = "Coderlink";

                var sheet = eP.Workbook.Worksheets.Add("Report");

                var rowNum = 1;

                // шапка
                sheet.Cells[rowNum, 1].Value = "Имя пользователя";
                sheet.Cells[rowNum, 2].Value = "Языки";
                sheet.Cells[rowNum, 3].Value = "Переведено";
                sheet.Cells[rowNum, 4].Value = "Утверждено";

                foreach (var row in reportRows)
                {
                    rowNum++;
                    sheet.Cells[rowNum, 1].Value = row.Name;
                    sheet.Cells[rowNum, 2].Value = row.Language;
                    sheet.Cells[rowNum, 3].Value = row.Translations;
                    sheet.Cells[rowNum, 4].Value = row.Confirmed;

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