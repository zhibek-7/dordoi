using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ITypeOfServiceRepository
    {
        /// <summary>
        /// Возвращает список тип услуг, содержащий только идентификатор и наименование.
        /// Для выборки, например checkbox.
        /// </summary>
        /// <returns>{id, name_text}</returns>
        Task<IEnumerable<TypeOfServiceForSelectDTO>> GetAllForSelectAsync();
    }
}
