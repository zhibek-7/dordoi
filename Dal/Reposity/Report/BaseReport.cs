using System;
using System.Collections.Generic;
using DAL.Context;
using Models.Reports;
using Remotion.Linq.Clauses;

namespace DAL.Reposity.Report
{
    /// <summary>
    /// Базовый класс для отчетов
    /// </summary>
    public abstract class BaseReport<T> where T : BaseReportRow
    {
        protected PostgreSqlNativeContext Context = PostgreSqlNativeContext.getInstance();

        /// <summary>
        /// Начальная дата
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Конечная дата
        /// </summary>
        public DateTime To { get; set; }

        /// <summary>
        /// Функция получения строк отчета удовлетворяющих датам
        /// </summary>
        /// <returns>Список строк отчета</returns>
        public abstract IEnumerable<T> Get();
    }
}
