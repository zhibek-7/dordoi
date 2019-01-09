using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    /// <summary>
    /// Базовый интерфейс репозиториев Async
    /// </summary>
    /// <typeparam name="T">Тип с которым работает репозиторий</typeparam>
    public interface IBaseRepositoryAsync<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
