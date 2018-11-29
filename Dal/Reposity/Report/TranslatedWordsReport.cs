using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Models.Reports;

namespace DAL.Reposity.Report
{
    public class TranslatedWordsReport: BaseReport<TranslatedWordsReportRow>
    {
        /// <summary>
        /// Функция получения строк отчета удовлетворяющих датам
        /// </summary>
        /// <returns>Список строк отчета</returns>
        public override IEnumerable<TranslatedWordsReportRow> Get()
        {
            using (IDbConnection dbConnection = Context.Connection)
            {
                dbConnection.Open();
                IEnumerable<TranslatedWordsReportRow> rows = 
                    dbConnection.Query<TranslatedWordsReportRow>(
                        "select " +
                        "u.\"Name\" as Name, " +
                        "l.\"Name\" as Language, " +
                        "count(t.\"ID_Locale\") as Translations, " +
                        "t.\"Confirmed\" as Confirmed" +
                        "from " +
                        "\"Translations\" t, " +
                        "\"Users\" u, " +
                        "\"Locales\" l, " +
                        "\"Strings\" s" +
                        "where t.\"ID_User\" = u.\"ID\" " +
                        "and t.\"ID_Locale\" = l.\"ID\" " +
                        "and t.\"ID_String\" = s.\"ID\" " +
                        "group by " +
                        "Name, " +
                        "Language, " +
                        "Confirmed");
                return rows;
            }
        }

       
    }
}
