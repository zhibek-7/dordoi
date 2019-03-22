using System;

namespace Models.DatabaseEntities
{
    [Serializable]
    public class FileTranslationInfo
    {

        public Guid LocaleId { get; set; }

        public double PercentOfTranslation { get; set; }

        public double PercentOfConfirmed { get; set; }

    }
}
