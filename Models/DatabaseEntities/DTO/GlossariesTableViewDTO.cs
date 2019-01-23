namespace Models.DatabaseEntities.DTO
{
    public class GlossariesTableViewDTO : BaseEntity
    {
        // public int ID { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        //public int? ID_File { get; set; }

        public string LocalesName { get; set; }

        public string LocalizationProjectsName { get; set; }

    }
}
