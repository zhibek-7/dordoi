using System;
using System.ComponentModel.DataAnnotations;

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
        public int  ID_User { get; set; }
        /// <summary>
        /// ИД вида деятельности 
        /// </summary>
        [Required]
        public int  ID_worktype { get; set; }
        /// <summary>
        /// Вид деятельности в виде строки
        /// </summary>
        public string Worktype { get; set; }
        /// <summary>
        /// Время активности. Устанавливается в БД автоматически
        /// </summary>
        [Required]
        public DateTime Datetime { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Ссылка на язык
        /// </summary>
        public int?  ID_Locale { get; set; }
        /// <summary>
        /// Ссылка на файл (для регистрации активности по добавлению, удалению и редактированию файлов
        /// </summary>
        public int? ID_File { get; set; }
        /// <summary>
        /// Ссылка на строку (для регистрации активности по добавлению, удалению и редактированию строк
        /// </summary>
        public int? ID_String { get; set; }
        /// <summary>
        /// Ссылка на перевод (для регистрации активности по добавлению, удалению и редактированию переводов
        /// </summary>
        public int? ID_Translation { get; set; }
        /// <summary>
        /// Ссылка на перевод (для регистрации активности по добавлению, удалению и редактированию проектов локализации
        /// </summary>
        public int? ID_Project { get; set; }

        public UserAction(){}

        public UserAction(int userId, string descript, int actionTypeID)
        {
            ID_User = userId;
            ID_worktype = actionTypeID;
            Description = descript;
            Datetime = DateTime.Now;
        }
    }
}
