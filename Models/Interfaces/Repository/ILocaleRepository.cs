using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using System.Threading.Tasks;
using Models.DatabaseEntities.DTO;

namespace Models.Interfaces.Repository
{
    public interface ILocaleRepository
    {
        //Task<IEnumerable<Locale>> GetAllAsync();
        Task<IEnumerable<Locale>> GetAllForProject(int projectId);
        //Task<IEnumerable<Locale>> GetByUserIdAsync(int userId);

        ///// <summary>
        ///// Возвращает назначенные языки перевода на проект локализации с процентами переводов по ним.
        ///// </summary>
        ///// <param name="projectId">Идентификатор проекта локализации.</param>
        ///// <returns></returns>
        //Task<IEnumerable<LocalizationProjectsLocalesDTO>> GetAllForProjectWithPercent(int projectId);

        Task CleanTableAsync();
        Task<bool> AddAsync(Locale newLocale);
    }
}
