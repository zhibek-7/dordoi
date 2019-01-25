using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DatabaseEntities
{
    public class Image : BaseEntity
    {
        /// <summary>
        /// ссылка на изображение
        /// </summary>
        [Required]
        public string URL { get; set; }

        /// <summary>
        /// Название изображения
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Дата добавления изображения
        /// </summary>
        [Required]
        public DateTime DateTimeAdded { get; set; }

        /// <summary>
        /// Пользователь, добавивший изображение
        /// </summary>
        public int ID_User { get; set; }

        /// <summary>
        /// Исхдные данные изображения
        /// </summary>
        [Required]
        public byte[] Data { get; set; }
    }
}
