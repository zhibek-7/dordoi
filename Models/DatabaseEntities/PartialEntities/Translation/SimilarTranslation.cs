﻿using System;

namespace Models.DatabaseEntities.PartialEntities.Translations
{
    [Serializable]
    public class SimilarTranslation : TranslationWithFile
    {
        /// <summary>
        /// Процент схожести
        /// </summary>
        public double similarity { get; set; }
    }
}
