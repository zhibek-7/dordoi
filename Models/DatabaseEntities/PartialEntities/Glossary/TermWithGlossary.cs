﻿using Models.DatabaseEntities;

namespace Models.DatabaseEntities.PartialEntities.Glossary
{
    public class TermWithGlossary : BaseEntity
    {
        /// <summary>
        /// Текст термина
        /// </summary>
        public string TermText { get; set; }

        /// <summary>
        /// Описание термина
        /// </summary>
        public string TermDesciption { get; set; }

        /// <summary>
        /// Глоссарий в котором содержится термин
        /// </summary>
        public int GlossaryId { get; set; }

        /// <summary>
        /// Глоссарий в котором содержится термин
        /// </summary>
        public string GlossaryName { get; set; }
    }
}
