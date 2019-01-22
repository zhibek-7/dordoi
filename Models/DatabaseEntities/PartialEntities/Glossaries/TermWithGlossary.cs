﻿using Models.DatabaseEntities;

namespace Models.DatabaseEntities.PartialEntities.Glossaries
{
    public class TermWithGlossary : Term
    {        
        /// <summary>
        /// Глоссарий в котором содержится термин
        /// </summary>
        public int GlossaryId { get; set; }

        /// <summary>
        /// Глоссарий в котором содержится термин
        /// </summary>
        public string GlossaryName { get; set; }

        /// <summary>
        /// Описание глоссария
        /// </summary>
        public string GlossaryDescription { get; set; }
    }
}