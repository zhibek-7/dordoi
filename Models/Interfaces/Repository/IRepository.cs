using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    /// <summary>
    /// Базовый интерфейс репозиториев
    /// </summary>
    /// <typeparam name="T">Тип с которым работает репозиторий</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        //void Add(T item);
        //bool Remove(int id);
        //void Update(T item);

        //T GetByID(int id);
        //IEnumerable<T> GetAll();
    }
}
