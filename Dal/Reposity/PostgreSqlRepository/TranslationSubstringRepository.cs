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
    public class TranslationSubstringRepository : IRepositoryAsync<Models.DatabaseEntities.TranslationSubstring>
    {
        private PostgreSqlNativeContext context;

        public TranslationSubstringRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Добавляет новую фразу
        /// </summary>
        /// <param name="item">Новая фраза</param>
        /// <returns>Кол-во добавленных фраз</returns>
        public Task<int> Add(Models.DatabaseEntities.TranslationSubstring item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получает все фразы
        /// </summary>
        /// <returns>Список фраз</returns>
        public async Task<IEnumerable<Models.DatabaseEntities.TranslationSubstring>> GetAll()
        {
            var query = "SELECT * FROM \"TranslationSubstrings\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Models.DatabaseEntities.TranslationSubstring> strings = await dbConnection.QueryAsync<Models.DatabaseEntities.TranslationSubstring>(query);
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
        public async Task<Models.DatabaseEntities.TranslationSubstring> GetByID(int id)
        {
            var query = "SELECT * " +
                        "FROM \"TranslationSubstrings\" " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var foundedString = await dbConnection.QuerySingleAsync<Models.DatabaseEntities.TranslationSubstring>(query, new { Id = id });
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

        public Task<bool> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Models.DatabaseEntities.TranslationSubstring item)
        {
            throw new NotImplementedException();
        }
    }
}
