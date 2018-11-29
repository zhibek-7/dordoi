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
        [Route("TranslatedWords")] //[Route("TranslatedWords/{dateFrom}")]
        public IEnumerable<TranslatedWordsReportRow> GetTranslatedWordsReport()//(DateTime dateFrom)
        {
            TranslatedWordsReport TranslatedWords = new TranslatedWordsReport();
            List<TranslatedWordsReportRow> reportRows = TranslatedWords.Get().ToList();
            return reportRows;
        }

    }
}