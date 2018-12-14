using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Models.Reports;

namespace DAL.Reposity.Report
{

    /// <summary>
    /// Для работы с БД
    /// </summary>
    public class TranslatedWordsReport: BaseReport<TranslatedWordsReportRow>
    {
        /// <summary>
        /// Функция получения строк отчета удовлетворяющих датам
        /// </summary>
        /// <returns>Список строк отчета</returns>
        public override IEnumerable<TranslatedWordsReportRow> GetRows(DateTime dateFrom, DateTime dateTo)
        {
            using (IDbConnection dbConnection = Context.Connection)
            {
                dbConnection.Open();
                var sqlQuery = "select " +
                               "u.\"Name\" as Name, " +
                               "l.\"Name\" as Language, " +
                               "count(t.\"ID_Locale\") as Translations, " +
                               "t.\"Confirmed\" as Confirmed " +
                               "from " +
                               "public.\"Translations\" t, " +
                               "public.\"Users\" u, " +
                               "public.\"Locales\" l, " +
                               "public.\"TranslationSubstrings\" s " +
                               "where " +
                               "t.\"ID_User\" = u.\"ID\" " +
                               "and t.\"ID_Locale\" = l.\"ID\" " +
                               "and t.\"ID_String\" = s.\"ID\" " +
                               "and t.\"DateTime\" > @dateFrom  " +
                               "and t.\"DateTime\" < @dateTo " +
                               "group by " +
                               "Name, " +
                               "Language, " +
                               "Confirmed";
                var rows = dbConnection.Query<TranslatedWordsReportRow>(sqlQuery, new { dateFrom, dateTo }).ToList();
                dbConnection.Close();
                return rows;
            }
        }       
    }
}
