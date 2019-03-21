using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class Image : BaseEntity
    {
        /// <summary>
        /// ссылка на изображение
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Название изображения
        /// </summary>
        [Required]
        public string Name_text { get; set; }

        /// <summary>
        /// Дата добавления изображения
        /// </summary>
        [Required]
        public DateTime Date_Time_Added { get; set; }

        /// <summary>
        /// Пользователь, добавивший изображение
        /// </summary>
        public Guid ID_User { get; set; }

        /// <summary>
        /// Исхдные данные изображения
        /// </summary>
        [Required]
        public byte[] body { get; set; }
    }
}
