﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IRepositoryAsync<T> where T: BaseEntity
    {
        Task<int> AddAsync(T item);
        Task<bool> RemoveAsync(int id);
        Task<bool> UpdateAsync(T item);

        Task<T> GetByIDAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}