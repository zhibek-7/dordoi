using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.Reports
{
    /// <summary>
    /// Строка отчета по количеству переведенных строк/слов
    /// </summary>
    public class TranslatedWordsReportRow : BaseReportRow
    {
        /// <summary>
        /// Имя пользователя осуществлявшего перевод
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Языки на которые переводил пользовател в указанный период времени
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public string workType { get; set; }

        /// <summary>
        /// Количество переводов
        /// </summary>
        public int Translations { get; set; }
    }
}
