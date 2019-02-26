using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using System.Threading.Tasks;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ILocalizationProjectRepository
    {
        void Add(LocalizationProject locale);
        LocalizationProject GetByID(int Id);
        IEnumerable<LocalizationProject> GetAll();
        Task<IEnumerable<LocalizationProjectForSelectDTO>> GetForSelectByUserAsync(string userName);
        bool Remove(int Id);
        void Update(LocalizationProject user);

        /// <summary>
        /// Возвращает проект локализации с подробной иформацией из связанных данных.
        /// </summary>
        /// <param name="id">Идентификатор проекта локализации.</param>
        /// <returns></returns>
        Task<LocalizationProject> GetWithDetailsById(int id);
    }
}
