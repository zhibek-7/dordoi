using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    [Serializable]
    class Project
    {
        /// <summary>
        /// Имя пользователя осуществлявшего перевод
        /// </summary>
        public string Name_text { get; set; }

        /// <summary>
        /// Языки на которые переводил пользовател в указанный период времени
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public string work_Type { get; set; }

        /// <summary>
        /// Количество переводов
        /// </summary>
        public int Translations { get; set; }
    }
}
