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
    public class TranslationSubstringRepository : IRepositoryAsync<TranslationSubstring>
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
        public Task<int> AddAsync(TranslationSubstring item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получает все фразы
        /// </summary>
        /// <returns>Список фраз</returns>
        public async Task<IEnumerable<TranslationSubstring>> GetAllAsync()
        {
            var query = "SELECT * FROM \"TranslationSubstrings\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> strings = await dbConnection.QueryAsync<Models.DatabaseEntities.TranslationSubstring>(query);
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
        /// Получает запись с конкретным id
        /// </summary>
        /// <param name="id">id необходимой фразы</param>
        /// <returns>Запись с необходимым id</returns>
        public async Task<TranslationSubstring> GetByIDAsync(int id)
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


        /// <summary>
        /// Фильтрует список строк по определенной фразе
        /// </summary>
        /// <param name="filtredString">фраза по которой происходит фильтрация</param>
        /// <param name="filtredListOfStrings">список строк среди которых происходит фильтрация</param>
        /// <returns>список строк содержащихся в списке строк </returns>
        public async Task<IEnumerable<TranslationSubstring>> FilterByString(string filtredString , IEnumerable<TranslationSubstring> filtredListOfStrings)
        {
            var query = "";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> filtredStrings = await dbConnection.QueryAsync<TranslationSubstring>(query);
                    dbConnection.Close();
                    return filtredStrings;
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
        /// Получает записи из определенного и открытых проектов
        /// </summary>
        /// <param name="fileId">id определенного проекта</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationSubstring>> GetStringsInVisibleAndCurrentProjectdAsync(int projectId)
        {
            var query = "SELECT * " +
                        "FROM \"TranslationSubstrings\" AS TS " +
                        "INNER JOIN \"Files\" AS F ON TS.\"ID_FileOwner\" = F.\"ID\" " +
                        "INNER JOIN \"LocalizationProjects\" AS LP " +
                        "WHERE LP.\"ID\" = @Id " +
                        "OR LP.\"Visibility\" = true ";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> strings = await dbConnection.QueryAsync<TranslationSubstring>(query, new { Id = projectId });
                    dbConnection.Close();
                    return strings;
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
        /// Получает записи из определенного файла по id файла
        /// </summary>
        /// <param name="fileId">id файла</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(int fileId)
        {
            var query = "SELECT TS.\"SubstringToTranslate\" AS \"SubstringToTranslate\", TS.\"Description\" AS \"Description\", " +
                        "TS.\"Context\" AS \"Context\", TS.\"TranslationMaxLength\" AS \"TranslationMaxLength\"," +
                        "TS.\"ID_FileOwner\" AS \"ID_FileOwner\", TS.\"Value\" AS \"Value\"," +
                        "TS.\"PositionInText\" AS \"PositionInText\", TS.\"ID\" AS \"ID\" " +
                        "FROM \"TranslationSubstrings\" AS TS " +
                        "INNER JOIN \"Files\" AS F ON TS.\"ID_FileOwner\" = F.\"ID\" " +
                        "WHERE F.\"ID\" = @Id";
            
            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> stringsInFile = await dbConnection.QueryAsync<TranslationSubstring>(query, new { Id = fileId });
                    dbConnection.Close();
                    return stringsInFile;
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

        public Task<bool> UpdateAsync(Models.DatabaseEntities.TranslationSubstring item)
        {
            throw new NotImplementedException();
        }
    }
}
