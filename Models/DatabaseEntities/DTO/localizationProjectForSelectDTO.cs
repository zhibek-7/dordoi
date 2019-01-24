namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// LocalizationProject {Id, Name} for example, for a drop-down list
    /// </summary>
    public class LocalizationProjectForSelectDTO : BaseEntity
    {
        public string Name { get; set; }
    }
}
