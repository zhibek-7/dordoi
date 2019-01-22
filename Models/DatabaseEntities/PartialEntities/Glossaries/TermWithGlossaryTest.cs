using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.PartialEntities.Glossaries
{
    public class TermWithGlossaryTest
    {
        /// <summary>
        /// Термин
        /// </summary>
        public Term Term { get; set; }

        /// <summary>
        /// Глоссарий термина
        /// </summary>
        public Glossary Glossary { get; set; }
    }
}
