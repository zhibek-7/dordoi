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
                        "group by " +
                        "Name, " +
                        "Language, " +
                        "Confirmed"
                        );
                return rows;
            }
            //List<TranslatedWordsReportRow> res = new List<TranslatedWordsReportRow>();
            //    res.Add(new TranslatedWordsReportRow(){Name = "Иван Иванов", Language = "Английский", Translations = 1200, Confirmed = true});
            //    res.Add(new TranslatedWordsReportRow() { Name = "Петр Петров", Language = "Французский", Translations = 1200, Confirmed = false });
            //    res.Add(new TranslatedWordsReportRow() { Name = "Никовай Николаев", Language = "Немецкий", Translations = 1200, Confirmed = true });
            //    res.Add(new TranslatedWordsReportRow() { Name = "Никита Никитин", Language = "Испанский", Translations = 1200, Confirmed = false });
            //return res;
        }       
    }
}
