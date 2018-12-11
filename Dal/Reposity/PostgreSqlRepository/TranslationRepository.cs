using System;
using System.Collections.Generic;
using System.Text;
using DAL.Context;
using System.Data;
using Dapper;
using Models.DatabaseEntities;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    /// <summary>
    /// Репозиторий для работы с вариантами перевода фраз
    /// </summary>
    public class TranslationRepository : IRepositoryAsync<Translation>
    {
        private PostgreSqlNativeContext context;

        public TranslationRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Функция добавления варианта перевода
        /// </summary>
        /// <param name="item">Вариант перевода</param>
        public async Task<int> Add(Translation item)
        {
            var query = "INSERT INTO \"Translations\" (\"ID_String\", \"Translated\", \"Confirmed\", \"ID_User\", \"DateTime\")" +
                        "VALUES (@ID_String, @Translated, @Confirmed, @ID_User, @DateTime)";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var numberOfInsertedRows = await dbConnection.ExecuteAsync(query, item);
                    dbConnection.Close();
                    return numberOfInsertedRows;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return 0;
            }
        }

        /// <summary>
        /// Функция получения всех переводов
        /// </summary>
        /// <returns>Список переводов</returns>
        public async Task<IEnumerable<Translation>> GetAll()
        {
            var query = @"SELECT * FROM 'Translations'";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Translation> translations = await dbConnection.QueryAsync<Translation>(query);
                    dbConnection.Close();
                    return translations;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
        }


        /// <summary>
        /// Функция получения варианта перевода по конкретному id
        /// </summary>
        /// <param name="id">id необходимого варианта перевода</param>
        /// <returns>Вариант перевода</returns>
        public Task<Translation> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Функция удаления варианта перевода по конкретному id
        /// </summary>
        /// <param name="id">id варианта перевода который нужно удалить</param>
        public async Task<bool> Remove(int id)
        {
            var query = "DELETE " +
                        "FROM \"Translations\" AS T " +
                        "WHERE T.\"ID\" = @id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var deletedRows = await dbConnection.ExecuteAsync(query, new { id });
                    dbConnection.Close();

                    return deletedRows > 0;
                }
            }            
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return false;
            }
        }

        /// <summary>
        /// Функция обновления варианта перевода
        /// </summary>
        /// <param name="item">Обновленный вариант перевода</param>
        public Task<bool> Update(Translation item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Функция получения всех вариантов перевода конкретной фразы
        /// </summary>
        /// <param name="idString">id фразы, варианты перевода которой необходимы</param>
        /// <returns>Список вариантов перевода</returns>
        public async Task<IEnumerable<Translation>> GetAllTranslationsInStringByID(int idString)
        {
            var query = "SELECT * " +
                        "FROM \"Translations\" " +
                        "WHERE \"ID_String\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Translation> translations = await dbConnection.QueryAsync<Translation>(query, new { Id = idString });
                    dbConnection.Close();
                    return translations;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
        }

    }
}
