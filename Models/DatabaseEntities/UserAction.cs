using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Модель для записи действий пользователей в системе
    /// </summary>
    public class UserAction : BaseEntity
    {
        /// <summary>
        /// ИД пользователя совершившего действие
        /// </summary>
        [Required]
        public int ID_User { get; set; }
        /// <summary>
        /// Имя пользователя совершившего действие
        /// </summary>
        [Required]
        public string User { get; set; }
        /// <summary>
        /// ИД вида деятельности 
        /// </summary>
        [Required]
        public int ID_worktype { get; set; }
        /// <summary>
        /// Вид деятельности в виде строки
        /// </summary>
        [Required]
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
        public int? ID_Locale { get; set; }
        /// <summary>
        /// Язык
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        /// Ссылка на файл (для регистрации активности по добавлению, удалению и редактированию файлов
        /// </summary>
        public int? ID_File { get; set; }
        /// <summary>
        /// Название файла (для регистрации активности по добавлению, удалению и редактированию файлов
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// Ссылка на строку (для регистрации активности по добавлению, удалению и редактированию строк
        /// </summary>
        public int? ID_String { get; set; }
        /// <summary>
        /// Содержимое строки (для регистрации активности по добавлению, удалению и редактированию строк
        /// </summary>
        public string String { get; set; }
        /// <summary>
        /// Ссылка на перевод (для регистрации активности по добавлению, удалению и редактированию переводов
        /// </summary>
        public int? ID_Translation { get; set; }
        /// <summary>
        /// Содержимое перевода (для регистрации активности по добавлению, удалению и редактированию переводов
        /// </summary>
        public string Translation { get; set; }
        /// <summary>
        /// Ссылка на проект (для регистрации активности по добавлению, удалению и редактированию проектов локализации
        /// </summary>
        public int? ID_Project { get; set; }
        /// <summary>
        /// Название проекта (для регистрации активности по добавлению, удалению и редактированию проектов локализации
        /// </summary>
        public string Project { get; set; }

        public UserAction() { }

        public UserAction(int userId, string userName, string descript, int actionTypeID, string actionName)
        {
            ID_User = userId;
            User = userName;
            ID_worktype = actionTypeID;
            Worktype = actionName;
            Description = descript;
            Datetime = DateTime.Now;
        }


    }
}
