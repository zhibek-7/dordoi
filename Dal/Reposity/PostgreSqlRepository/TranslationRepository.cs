using System;
using System.Collections.Generic;
using System.Text;
using DAL.Context;
using System.Data;
using Dapper;
using Models.DatabaseEntities;
using System.Linq;
using System.Threading.Tasks;
using Models.Translations;
using Models.Interfaces.Repository;

namespace DAL.Reposity.PostgreSqlRepository
{
    /// <summary>
    /// Репозиторий для работы с вариантами перевода фраз
    /// </summary>
    public class TranslationRepository : BaseRepository, IRepositoryAsync<Translation>
    {
        private PostgreSqlNativeContext context;

        public TranslationRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Метод добавления варианта перевода
        /// </summary>
        /// <param name="item">Вариант перевода</param>
        public async Task<int> AddAsync(Translation item)
        {
            var query = "INSERT INTO \"Translations\" (\"ID_String\", \"Translated\", \"Confirmed\", \"ID_User\", \"DateTime\", \"ID_Locale\")" +
                        "VALUES (@ID_String, @Translated, @Confirmed, @ID_User, @DateTime, @ID_Locale) " +
                        "RETURNING  \"Translations\".\"ID\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<int>(query, item);
                    dbConnection.Close();
                    return idOfInsertedRow;
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
        /// Метод получения всех переводов
        /// </summary>
        /// <returns>Список переводов</returns>
        public async Task<IEnumerable<Translation>> GetAllAsync()
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
        /// Метод получения варианта перевода по конкретному id
        /// </summary>
        /// <param name="id">id необходимого варианта перевода</param>
        /// <returns>Вариант перевода</returns>
        public async Task<Translation> GetByIDAsync(int id)
        {
            var query = "SELECT * FROM \"Translations\" WHERE \"ID\" = @id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var translation = await dbConnection.QuerySingleOrDefaultAsync<Translation>(query, new { id });
                    dbConnection.Close();

                    return translation;
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
        /// Метод удаления варианта перевода по конкретному id
        /// </summary>
        /// <param name="id">id варианта перевода который нужно удалить</param>
        public async Task<bool> RemoveAsync(int id)
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
        /// Метод обновления варианта перевода
        /// </summary>
        /// <param name="item">Обновленный вариант перевода</param>
        public async Task<bool> UpdateAsync(Translation item)
        {
            try
            {
                using (var dbConnection = this.context.Connection)
                {
                    dbConnection.Open();
                    var updateTranslationSql =
                        "UPDATE \"Translations\" SET " +
                        "\"ID_String\"=@ID_String, " +
                        "\"Translated\"=@Translated, " +
                        "\"Confirmed\"=@Confirmed, " +
                        "\"ID_User\"=@ID_User, " +
                        "\"DateTime\"=@DateTime, " +
                        "\"ID_Locale\"=@ID_Locale " +
                        "WHERE \"ID\"=@ID";
                    var updateTranslationParam = item;
                    this.LogQuery(updateTranslationSql, updateTranslationParam);
                    await dbConnection.ExecuteAsync(
                        sql: updateTranslationSql,
                        param: updateTranslationParam);
                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn("Exception on trying to update translation.", exception);
                return false;
            }
        }

        /// <summary>
        /// Метод получения всех вариантов перевода конкретной фразы
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

        /// <summary>
        /// Принять перевод
        /// </summary>
        /// <param name="idTranslation">id перевода</param>
        /// <returns></returns>
        public async Task<bool> AcceptTranslation(int idTranslation)
        {
            var query = "UPDATE \"Translations\" " +
                        "SET \"Confirmed\" = true " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var updatedRows = await dbConnection.ExecuteAsync(query, new { Id = idTranslation });
                    dbConnection.Close();

                    return updatedRows > 0;
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
        /// Отклонить перевод
        /// </summary>
        /// <param name="idTranslation">id Перевода</param>
        /// <returns></returns>
        public async Task<bool> RejectTranslation(int idTranslation)
        {
            var query = "UPDATE \"Translations\" " +
                        "SET \"Confirmed\" = false " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var updatedRows = await dbConnection.ExecuteAsync(query, new { Id = idTranslation });
                    dbConnection.Close();

                    return updatedRows > 0;
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
        /// Метод получения всех вариантов перевода заданой фразы в проекте локализации
        /// </summary>
        /// <param name="translationText">Фраза по которой необходимо найти переводы</param>
        /// <returns>Список вариантов перевода</returns>
        public async Task<IEnumerable<TranslationWithFile>> GetAllTranslationsByMemory(int currentProjectId, string translationText)
        {
            var query = "SELECT F.\"Name\" AS \"FileOwnerName\", T.\"Translated\" AS \"TranslationVariant\", " +
                        "TS.\"SubstringToTranslate\" AS \"TranslationText\" " +
                        "FROM \"LocalizationProjects\" AS LP " +
                        "INNER JOIN \"Files\" AS F ON F.\"ID_LocalizationProject\" = LP.\"ID\" " +
                        "INNER JOIN \"TranslationSubstrings\" AS TS ON TS.\"ID_FileOwner\" = F.\"ID\" " +
                        "INNER JOIN \"Translations\" AS T ON T.\"ID_String\" = TS.\"ID\" " +
                        "WHERE TS.\"SubstringToTranslate\" LIKE @TranslationText";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationWithFile> translations = await dbConnection.QueryAsync<TranslationWithFile>(query, new { TranslationText = "%" + translationText + "%" });
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
        /// Метод получения схожих вариантов перевода в проекте локализации
        /// </summary>
        /// <param name="currentProjectId">id проекта в котором происходит поиск</param>
        /// <param name="translationSubstring">фраза для которой происходит поиск совпадений</param>
        /// <returns></returns>
        public async Task<IEnumerable<SimilarTranslation>> GetSimilarTranslationsAsync(int currentProjectId, TranslationSubstring translationSubstring)
        {
            var query = "SELECT \"SubstringToTranslate\" AS \"TranslationText\", similarity(\"SubstringToTranslate\", @TranslationSubstringText) AS \"Similarity\", " +
                        "\"Files\".\"Name\" AS \"FileOwnerName\", \"Translations\".\"Translated\" AS \"TranslationVariant\"" +
                        "FROM \"LocalizationProjects\" " +
                        "INNER JOIN \"Files\" ON \"Files\".\"ID_LocalizationProject\" = \"LocalizationProjects\".\"ID\" " +
                        "INNER JOIN \"TranslationSubstrings\" ON \"TranslationSubstrings\".\"ID_FileOwner\" = \"Files\".\"ID\" " +
                        "INNER JOIN \"Translations\" ON \"Translations\".\"ID_String\" = \"TranslationSubstrings\".\"ID\" " +
                        "WHERE (\"LocalizationProjects\".\"ID\" = @ProjectId " +
                        "AND \"SubstringToTranslate\" % @TranslationSubstringText " +
                        "AND \"TranslationSubstrings\".\"ID\" != @TranslationSubstringId);";


            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<SimilarTranslation> similarTranslations = await dbConnection.QueryAsync<SimilarTranslation>(query,
                        new { TranslationSubstringText = translationSubstring.SubstringToTranslate, TranslationSubstringId = translationSubstring.ID, ProjectId = currentProjectId });
                    dbConnection.Close();
                    return similarTranslations;
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
