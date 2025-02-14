﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Язык перевода назначенный на проект локализации с процентом перевода
    /// </summary>
    [Serializable]
    public class LocalizationProjectsLocalesDTO : BaseEntity
    {
        /// <summary>
        /// Идентификатор языка
        /// </summary>
        public Guid Locale_Id { get; set; }
        /// <summary>
        /// Наименование языка
        /// </summary>
        public string Locale_Name { get; set; }
        /// <summary>
        /// Путь к изображению флага
        /// </summary>
        public string Locale_Url { get; set; }

        /// <summary>
        /// Процент переведенных слов
        /// </summary>
        public string Percent_Of_Translation { get; set; }
        /// <summary>
        /// Процент подтвержденных переводов
        /// </summary>
        public string Percent_Of_Confirmed { get; set; }
    }
}
