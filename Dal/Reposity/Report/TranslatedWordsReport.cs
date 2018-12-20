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

        /// <summary>
        /// Функция получения строк отчета удовлетворяющих параметрам
        /// </summary>
        /// <param name="projectID">Идентификатор проекта локализации</param>
        /// <param name="dateFrom">Начальная дата выбоки</param>
        /// <param name="dateTo">Конечная дата выборки</param>
        /// <param name="userId">ID пользователя</param>
        /// <param name="localeId">ID языка перевода</param>
        /// <param name="countByChar">Считать по символам с пробелами? Если false то по словам</param>
        /// <param name="countTranslations">Считать по переводам? Если false то по исходным строкам</param>
        /// <returns>Список строк отчета</returns>
        public IEnumerable<TranslatedWordsReportRow> GetRowsWithFilter
            (int projectID, 
            DateTime dateFrom,
            DateTime dateTo, 
            int userId = -1, 
            int localeId = -1, 
            bool countByChar = true, 
            bool countTranslations = true)
        {
            using (var dbConnection = Context.Connection)
            {
                dbConnection.Open(); 
                var sqlQuery = "SELECT name_ as Name, lang_ as language, worktype_ as workType, count_ as Translations" +
                               $" FROM TranslatedWords({projectID}, '{dateFrom.ToString("dd-MM-yyyy")}', '{dateTo.ToString("dd-MM-yyyy")}', {userId}, {localeId}, {countByChar}, {countTranslations});";
                var rows = dbConnection.Query<TranslatedWordsReportRow>(sqlQuery, new { dateFrom, dateTo }).ToList();
                dbConnection.Close();
                return rows;
            }
        }
    }
}
