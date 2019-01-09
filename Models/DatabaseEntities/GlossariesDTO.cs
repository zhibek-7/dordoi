using Models.DatabaseEntities;

namespace Models.DatabaseEntities
{
    public class GlossariesDTO : BaseEntity
    {
        // public int ID { get; set; }
        public string Name { get; set; }

        public string LocalesName { get; set; }

        public string LocalizationProjectsName { get; set; }

    }
}
