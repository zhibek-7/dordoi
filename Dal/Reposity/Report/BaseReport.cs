using System;
using System.Collections.Generic;
using DAL.Context;
using Models.Reports;

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
    }
}
