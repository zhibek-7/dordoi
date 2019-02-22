﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    /// <summary>
    /// Память переводов со связанными данными без группировки по объектам.
    /// Для получения результата запроса.
    /// </summary>
    [Serializable]
    public class TranslationMemory : BaseEntity
    {
        public string name_text { get; set; }
        public int? id_file { get; set; }

        public int? string_count { get; set; }

        public int? locale_id { get; set; }
        public string locale_name { get; set; }

        public int? localization_project_id { get; set; }
        public string localization_project_name { get; set; }
    }
}
