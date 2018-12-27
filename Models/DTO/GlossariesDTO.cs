using Models.DatabaseEntities;

namespace Models.DTO
{
    public class GlossariesDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }   
        
        public string LocalesName { get; set; } 
        
        public string LocalizationProjectsName { get; set; }
        
    }
}
