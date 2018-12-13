using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace DAL
{
    public interface IRepositoryAsync<T> where T: BaseEntity
    {
        Task<int> Add(T item);
        Task<bool> Remove(int id);
        Task<bool> Update(T item);

        Task<T> GetByID(int id);
        Task<IEnumerable<T>> GetAll();
    }
}
