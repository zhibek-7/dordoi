namespace Models.DatabaseEntities.DTO
{
    /// <summary>
    /// Проект локализации, содержащий только идентификатор и наименование.
    /// Для выборки, например checkbox.
    /// </summary>
    public class LocalizationProjectForSelectDTO : BaseEntity
    {
        public string Name_text { get; set; }
    }
}
