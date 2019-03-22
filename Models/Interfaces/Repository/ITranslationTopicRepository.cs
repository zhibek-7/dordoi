using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ITranslationTopicRepository
    {
        /// <summary>
        /// Возвращает список тематик, содержащий только идентификатор и наименование.
        /// Для выборки, например checkbox.
        /// </summary>
        /// <returns>{id, name_text}</returns>
        Task<IEnumerable<TranslationTopicForSelectDTO>> GetAllForSelectAsync();
    }
}
