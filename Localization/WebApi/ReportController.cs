using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Reports;

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
    }
}