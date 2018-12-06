using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;

namespace DAL.Reposity
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Add(T item);
        void Remove(int id);
        void Update(T item);
        T GetByID(int id);
        IEnumerable<T> GetAll();
    }
}
