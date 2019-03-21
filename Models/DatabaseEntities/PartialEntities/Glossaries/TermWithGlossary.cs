using System;
using Models.DatabaseEntities;

namespace Models.DatabaseEntities.PartialEntities.Glossaries
{
    [Serializable]
    public class TermWithGlossary : Term
    {
        /// <summary>
        /// Глоссарий в котором содержится термин
        /// </summary>
        public Guid Glossary_Id { get; set; }

        /// <summary>
        /// Глоссарий в котором содержится термин
        /// </summary>
        public string Glossary_Name { get; set; }

        /// <summary>
        /// Описание глоссария
        /// </summary>
        public string Glossary_Description { get; set; }
    }
}
