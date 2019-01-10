using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;

namespace Models.DatabaseEntities.Translations
{
    public class TranslationWithFile
    {
        public string FileOwnerName { get; set; }

        public string TranslationText { get; set; }

        public string TranslationVariant { get; set; }
    }
}
