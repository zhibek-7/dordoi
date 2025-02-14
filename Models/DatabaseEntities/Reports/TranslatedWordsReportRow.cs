﻿
using System;

namespace Models.DatabaseEntities.Reports
{
    /// <summary>
    /// Строка отчета по количеству переведенных строк/слов
    /// </summary>
    [Serializable]
    public class TranslatedWordsReportRow : BaseReportRow
    {
        /// <summary>
        /// Имя пользователя осуществлявшего перевод
        /// </summary>
        public string name_text { get; set; }

        /// <summary>
        /// Языки на которые переводил пользовател в указанный период времени
        /// </summary>
        public string language { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public string work_Type { get; set; }

        /// <summary>
        /// Количество переводов
        /// </summary>
        public int translations { get; set; }
    }
}
