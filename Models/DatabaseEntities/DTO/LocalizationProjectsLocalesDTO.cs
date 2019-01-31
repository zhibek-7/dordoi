using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Язык перевода назначенный на проект локализации с процентом перевода
    /// </summary>
    public class LocalizationProjectsLocalesDTO : BaseEntity
    {
        /// <summary>
        /// Наименование языка
        /// </summary>
        public string LocaleName { get; set; }
        /// <summary>
        /// Путь к изображению флага
        /// </summary>
        public string LocaleUrl { get; set; }

        /// <summary>
        /// Процент переведенных слов
        /// </summary>
        public string PercentOfTranslation { get; set; }
        /// <summary>
        /// Процент подтвержденных переводов
        /// </summary>
        public string PercentOfConfirmed { get; set; }
    }
}
