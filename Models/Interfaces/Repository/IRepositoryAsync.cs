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
    public interface IRepositoryAsync<T> where T : BaseEntity
    {
        Task<Guid?> AddAsync(T item);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> UpdateAsync(T item);

        Task<T> GetByIDAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
