using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Модель для записи действий пользователей в системе
    /// </summary>
    public class UserAction: BaseEntity
    {
        /// <summary>
        /// ИД пользователя совершившего действие
        /// </summary>
        [Required]
        public string ID_User { get; set; }
        /// <summary>
        /// ИД вида деятельности 
        /// </summary>
        [Required]
        public string ID_worktype { get; set; }
        /// <summary>
        /// Время активности. Устанавливается в БД автоматически
        /// </summary>
        [Required]
        public string Datetime { get; set; }
        /// <summary>
        /// Описание.
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Ссылка на язык
        /// </summary>
        public string ID_Locale { get; set; }
        /// <summary>
        /// Ссылка на файл (для регистрации активности по добавлению, удалению и редактированию файлов
        /// </summary>
        public string ID_File { get; set; }
        /// <summary>
        /// Ссылка на строку (для регистрации активности по добавлению, удалению и редактированию строк
        /// </summary>
        public string ID_String { get; set; }
        /// <summary>
        /// Ссылка на перевод (для регистрации активности по добавлению, удалению и редактированию переводов
        /// </summary>
        public string ID_Translation { get; set; }
        /// <summary>
        /// Ссылка на перевод (для регистрации активности по добавлению, удалению и редактированию проектов локализации
        /// </summary>
        public string ID_Project { get; set; }
    }
}
