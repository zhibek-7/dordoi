using System;
using System.Collections.Generic;
using System.Text;
using DAL.Context;
using System.Data;
using Dapper;
using Models.DatabaseEntities;
using System.Linq;
using System.Threading.Tasks;

using Models.Interfaces.Repository;
using Models.DatabaseEntities.PartialEntities.Translations;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    /// <summary>
    /// Репозиторий для работы с вариантами перевода фраз
    /// </summary>
    public class TranslationRepository : BaseRepository, IRepositoryAsync<Translation>
    {
        public TranslationRepository(string connectionStr) : base(connectionStr)
        {
        }

        /// <summary>
        /// Метод добавления варианта перевода
        /// </summary>
        /// <param name="item">Вариант перевода</param>
        public async Task<int> AddAsync(Translation item)
        {
            var query = "INSERT INTO translations (id_string, translated, confirmed, id_user, datetime, id_locale)" +
                        "VALUES (@ID_String, @Translated, @Confirmed, @ID_User, @DateTime, @ID_Locale) " +
                        "RETURNING  translations.id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query, item);
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<int>(query, item);
                    return idOfInsertedRow;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        /// <summary>
        /// Метод получения всех переводов
        /// </summary>
        /// <returns>Список переводов</returns>
        public async Task<IEnumerable<Translation>> GetAllAsync()
        {
            var query = @"SELECT * FROM translations";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query);
                    IEnumerable<Translation> translations = await dbConnection.QueryAsync<Translation>(query);
                    return translations;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
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
            var query = "SELECT * FROM translations WHERE id = @id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(query, param);
                    var translation = await dbConnection.QuerySingleOrDefaultAsync<Translation>(query, param);
                    return translation;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
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
                        "FROM translations AS T " +
                        "WHERE T.id = @id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(query, param);
                    var deletedRows = await dbConnection.ExecuteAsync(query, param);
                    return deletedRows > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.RemoveAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.RemoveAsync)} {nameof(Exception)} ",
                    exception);
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
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var updateTranslationSql =
                        "UPDATE translations SET " +
                        "id_string=@ID_String, " +
                        "translated=@Translated, " +
                        "confirmed=@Confirmed, " +
                        "id_user=@ID_User, " +
                        "datetime=@DateTime, " +
                        "id_locale=@ID_Locale " +
                        "WHERE id=@ID";
                    var updateTranslationParam = item;
                    this.LogQuery(updateTranslationSql, updateTranslationParam);
                    await dbConnection.ExecuteAsync(
                        sql: updateTranslationSql,
                        param: updateTranslationParam);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.UpdateAsync)} {nameof(NpgsqlException)} " + "Exception on trying to update translation.",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.UpdateAsync)} {nameof(Exception)} " + "Exception on trying to update translation.",
                    exception);
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
                        "FROM translations " +
                        "WHERE id_string = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = idString };
                    this.LogQuery(query, param);
                    IEnumerable<Translation> translations = await dbConnection.QueryAsync<Translation>(query, param);
                    return translations;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllTranslationsInStringByID)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllTranslationsInStringByID)} {nameof(Exception)} ",
                    exception);
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
            var query = "UPDATE translations " +
                        "SET confirmed = true " +
                        "WHERE id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = idTranslation };
                    this.LogQuery(query, param);
                    var updatedRows = await dbConnection.ExecuteAsync(query, param);
                    return updatedRows > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.AcceptTranslation)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.AcceptTranslation)} {nameof(Exception)} ",
                    exception);
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
            var query = "UPDATE translations " +
                        "SET confirmed = false " +
                        "WHERE id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = idTranslation };
                    this.LogQuery(query, param);
                    var updatedRows = await dbConnection.ExecuteAsync(query, param);
                    return updatedRows > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.RejectTranslation)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.RejectTranslation)} {nameof(Exception)} ",
                    exception);
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
            var query = "SELECT F.name_text AS fileownername, T.translated AS translationvariant, " +
                        "TS.substring_to_translate AS tTranslationtext " +
                        "FROM localization_projects AS LP " +
                        "INNER JOIN files AS F ON F.id_localization_project = LP.id " +
                        "INNER JOIN translation_substrings AS TS ON TS.id_file_owner = F.id " +
                        "INNER JOIN translations AS T ON T.id_string = TS.id " +
                        "WHERE TS.substring_to_translate LIKE @TranslationText";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { TranslationText = "%" + translationText + "%" };
                    this.LogQuery(query, param);
                    IEnumerable<TranslationWithFile> translations = await dbConnection.QueryAsync<TranslationWithFile>(query, param);
                    return translations;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllTranslationsByMemory)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllTranslationsByMemory)} {nameof(Exception)} ",
                    exception);
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
            var query = "SELECT substring_to_translate AS translationtext, similarity(substring_to_translate, @TranslationSubstringText) AS similarity, " +
                        "files.name_text AS fileownername, translations.translated AS translationvariant" +
                        "FROM localization_projects " +
                        "INNER JOIN files ON files.id_localization_project = localization_projects.id " +
                        "INNER JOIN translation_substrings ON translation_substrings.id_file_owner = files.id " +
                        "INNER JOIN translations ON translations.id_string = translation_substrings.id " +
                        "WHERE (localization_projects.id = @ProjectId " +
                        "AND substring_to_translate % @TranslationSubstringText " +
                        "AND translation_substrings.id != @TranslationSubstringId);";


            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { TranslationSubstringText = translationSubstring.Substring_To_Translate, TranslationSubstringId = translationSubstring.ID, ProjectId = currentProjectId };
                    this.LogQuery(query, param);
                    IEnumerable<SimilarTranslation> similarTranslations = await dbConnection.QueryAsync<SimilarTranslation>(query, param);
                    return similarTranslations;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetSimilarTranslationsAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetSimilarTranslationsAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

    }
}
