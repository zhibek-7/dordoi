using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Models.DatabaseEntities.Reports;
using Npgsql;

namespace DAL.Reposity.Report
{

    /// <summary>
    /// Для работы с БД
    /// </summary>
    public class TranslatedWordsReport : BaseReport<TranslatedWordsReportRow>
    {
        protected string connectionString;

        public TranslatedWordsReport(string connectionStr)
        {
            this.connectionString = connectionStr;
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
        public IEnumerable<TranslatedWordsReportRow> GetRows
            (Guid projectID,
            DateTime dateFrom,
            DateTime dateTo,
            Guid userId,
            Guid localeId,
            bool countByChar = true,
            bool countTranslations = true)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                //string userIdNew = null;
                //if (userId.Equals(Guid.Empty) == false)
                // {
                //    userIdNew = userId.ToString();
                //}

                //string localeIdNew = null;
                //if (localeId.Equals(Guid.Empty) == false)
                //{
                //   localeIdNew = localeId.ToString();
                //}

                var sqlQuery = "SELECT name_ as name_text, lang_ as language, worktype_ as work_Type, count_ as translations" +
                               $" FROM translatedwords('{projectID}', '{dateFrom.ToString("dd-MM-yyyy")}', '{dateTo.ToString("dd-MM-yyyy")}', '{userId}', '{localeId}', {countByChar}, {countTranslations});";
                var rows = dbConnection.Query<TranslatedWordsReportRow>(sqlQuery, new { dateFrom, dateTo }).ToList();
                return rows;
            }
        }
    }
}
