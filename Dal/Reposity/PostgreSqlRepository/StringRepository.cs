using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class StringRepository : IRepositoryAsync<Models.DatabaseEntities.String>
    {
        private PostgreSqlNativeContext context;

        public StringRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Добавляет новую фразу
        /// </summary>
        /// <param name="item">Новая фраза</param>
        /// <returns>Кол-во добавленных фраз</returns>
        public Task<int> AddAsync(Models.DatabaseEntities.String item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получает все фразы
        /// </summary>
        /// <returns>Список фраз</returns>
        public async Task<IEnumerable<Models.DatabaseEntities.String>> GetAllAsync()
        {
            var query = "SELECT * FROM \"TranslationSubstrings\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Models.DatabaseEntities.String> strings = await dbConnection.QueryAsync<Models.DatabaseEntities.String>(query);
                    dbConnection.Close();
                    return strings;
                }
            }
            catch(Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
            
        }

        /// <summary>
        /// Получает запись с конкретным 
        /// </summary>
        /// <param name="id">id необходимой фразы</param>
        /// <returns>Запись с необходимым id</returns>
        public async Task<Models.DatabaseEntities.String> GetByIDAsync(int id)
        {
            var query = "SELECT * " +
                        "FROM \"TranslationSubstrings\" " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var foundedString = await dbConnection.QuerySingleAsync<Models.DatabaseEntities.String>(query, new { Id = id });
                    dbConnection.Close();
                    return foundedString;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
            
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Models.DatabaseEntities.String item)
        {
            throw new NotImplementedException();
        }
    }
}
