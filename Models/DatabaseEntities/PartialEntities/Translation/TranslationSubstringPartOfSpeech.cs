using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.PartialEntities.Translation
{
    [Serializable]
    public class TranslationSubstringPartOfSpeech
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid id_glossary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid id_part_of_speech { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid id_string { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid t_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid t_id_locale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ts_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ts_substring_to_translate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ps_name_text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string t_translated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l_code { get; set; }

    }
}
