using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using Models.DatabaseEntities;
using System.Linq;
using System.Threading.Tasks;
using Models.Interfaces.Repository;
using Models.DatabaseEntities.PartialEntities.Translations;
using Npgsql;
using SqlKata;
using Models.DatabaseEntities.PartialEntities.Translation;

namespace DAL.Reposity.PostgreSqlRepository
{
    /// <summary>
    /// Репозиторий для работы с вариантами перевода фраз
    /// </summary>
    public class TranslationRepository : BaseRepository, IRepositoryAsync<Translation>
    {
        private UserActionRepository _action;

        public TranslationRepository(string connectionStr) : base(connectionStr)
        {
            _action = new UserActionRepository(connectionStr);
        }

        /// <summary>
        /// Метод добавления варианта перевода
        /// </summary>
        /// <param name="item">Вариант перевода</param>
        public async Task<Guid?> AddAsync(Translation item)
        {
            var query = "INSERT INTO translations (id_string, translated, confirmed, id_user, datetime, id_locale)" +
                        "VALUES (@ID_String, @Translated, @Confirmed, @ID_User, @DateTime, @ID_Locale) " +
                        "RETURNING  translations.id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query, item.GetType(), item);
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<Guid>(query, item);

                    /*Логироание*/
                    _action.AddAddTraslationActionAsync(item, idOfInsertedRow, WorkTypes.AddTraslation);
                    /**/

                    return idOfInsertedRow;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return null;
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
        public async Task<Translation> GetByIDAsync(Guid id)
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
        public async Task<bool> RemoveAsync(Guid id)
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
                        "WHERE id=@id";
                    var updateTranslationParam = item;
                    this.LogQuery(updateTranslationSql, updateTranslationParam.GetType(), updateTranslationParam);
                    await dbConnection.ExecuteAsync(
                        sql: updateTranslationSql,
                        param: updateTranslationParam);

                    /*Логироание*/
                    _action.AddAddTraslationActionAsync(item, item.id, WorkTypes.UpdateTranslation);
                    /**/

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
        public async Task<IEnumerable<Translation>> GetAllTranslationsInStringByID(Guid idString)
        {
            var query = "SELECT t.id_string, " +
                        "t.translated, " +
                        "t.confirmed, " +
                        "t.id_user, " +
                        "t.datetime, " +
                        "t.id_locale, " +
                        "t.selected, " +
                        "t.id, " +
                        "u.name_text AS User_Name " +
                        "FROM translations as t " +
                        "INNER JOIN users AS u ON u.id = t.id_user " +
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
        /// Метод получения всех вариантов перевода конкретной фразы с учётом языка
        /// </summary>
        /// <param name="idString">id фразы, варианты перевода которой необходимы</param>
        /// <returns>Список вариантов перевода</returns>
        public async Task<IEnumerable<Translation>> GetAllTranslationsInStringByIDByLocale(Guid idString, Guid localeId)
        {
            var query = "SELECT t.id_string, " +
                        "t.translated, " +
                        "t.confirmed, " +
                        "t.id_user, " +
                        "t.datetime, " +
                        "t.id_locale, " +
                        "t.selected, " +
                        "t.id, " +
                        "u.name_text AS User_Name " +
                        "FROM translations as t " +
                        "INNER JOIN users AS u ON u.id = t.id_user " +
                        "WHERE id_string = @idString AND id_locale = @localeId";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { idString, localeId };
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
        public async Task<bool> AcceptTranslation(Guid idTranslation, bool selectTranslation = false)
        {
            var query = "UPDATE translations " +
                        "SET confirmed = true " +
                        "WHERE id = @Id";

            var querySelectedTranslation =
                        "UPDATE translations " +
                        "SET confirmed = true, selected = true " +
                        "WHERE id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = idTranslation, selectTranslation };
                    this.LogQuery(query, param);

                    if (selectTranslation)
                    {
                        var updatedRows = await dbConnection.ExecuteAsync(querySelectedTranslation, param);

                        return updatedRows > 0;
                    }
                    else
                    {
                        var updatedRows = await dbConnection.ExecuteAsync(query, param);

                        return updatedRows > 0;
                    }
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
        public async Task<bool> RejectTranslation(Guid idTranslation, bool selectTranslation = false)
        {
            var query = "UPDATE translations " +
                        "SET confirmed = false " +
                        "WHERE id = @Id";

            var querySelectedTranslation =
                       "UPDATE translations " +
                       "SET confirmed = false, selected = false " +
                       "WHERE id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var param = new { Id = idTranslation, selectTranslation };
                    this.LogQuery(query, param);

                    if (selectTranslation)
                    {
                        var updatedRows = await dbConnection.ExecuteAsync(querySelectedTranslation, param);

                        return updatedRows > 0;
                    }
                    else
                    {
                        var updatedRows = await dbConnection.ExecuteAsync(query, param);

                        return updatedRows > 0;
                    }
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
        public async Task<IEnumerable<TranslationWithFile>> GetAllTranslationsByMemory(Guid currentProjectId, string translationText)
        {
            var query = "SELECT F.name_text AS file_Owner_Name, T.translated AS translation_Variant, " +
                        "TS.substring_to_translate AS translation_Text " +
                        "FROM localization_projects AS LP " +
                        "INNER JOIN files AS F ON F.id_localization_project = LP.id " +
                        "INNER JOIN translation_substrings AS TS ON TS.id_file_owner = F.id " +
                        "INNER JOIN translations AS T ON T.id_string = TS.id " +
                        "WHERE  LOWER(TS.substring_to_translate) LIKE LOWER(@TranslationText)";

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
        public async Task<IEnumerable<SimilarTranslation>> GetSimilarTranslationsAsync(Guid currentProjectId, Guid localeId, TranslationSubstring translationSubstring)
        {
            var query = "SELECT substring_to_translate AS translation_text, similarity(substring_to_translate, @TranslationSubstringText) AS similarity, " +
                        "files.name_text AS file_owner_name, translations.translated AS translation_variant" +
                        " FROM localization_projects " +
                        "INNER JOIN files ON files.id_localization_project = localization_projects.id " +
                        "INNER JOIN translation_substrings ON translation_substrings.id_file_owner = files.id " +
                        "INNER JOIN translations ON translations.id_string = translation_substrings.id " +
                        "WHERE (localization_projects.id = @ProjectId " +
                        "AND substring_to_translate % @TranslationSubstringText " +
                        "AND translation_substrings.id != @TranslationSubstringId " +
                        "AND translations.id_locale = @localeId);";


            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { TranslationSubstringText = translationSubstring.substring_to_translate, TranslationSubstringId = translationSubstring.id, ProjectId = currentProjectId, localeId };
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

        public async Task<IEnumerable<TranslationWithLocaleText>> GetTranslationsInOtherLanguages(
            Guid currentProjectId,
            Guid translationSubstringId,
            Guid localeId)
        {
            var query = "SELECT " +
                        "translations.translated AS translation_text, " +
                        "locales.name_text AS locale_name_text " +
                        "FROM files " +
                        "INNER JOIN translation_substrings ON translation_substrings.id_file_owner = files.id " +
                        "INNER JOIN translations ON translations.id_string = translation_substrings.id " +
                        "INNER JOIN locales ON locales.id = translations.id_locale " +
                        "WHERE (files.id_localization_project = @currentProjectId " +
                        "AND translation_substrings.id = @translationSubstringId " +
                        "AND translations.id_locale != @localeId);";


            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { currentProjectId, translationSubstringId, localeId };
                    this.LogQuery(query, param);
                    IEnumerable<TranslationWithLocaleText> translationsInOtherLanguages = await dbConnection.QueryAsync<TranslationWithLocaleText>(query, param);
                    return translationsInOtherLanguages;
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

        /// <summary>
        /// Возвращает все варианты перевода конкретной фразы с языком перевода
        /// </summary>
        /// <param name="idString">id фразы</param>
        /// <returns>Список вариантов перевода</returns>
        public async Task<IEnumerable<TranslationDTO>> GetAllTranslationsInStringWithLocaleByID(Guid idString)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translations")
                        .LeftJoin("locales", "locales.id", "translations.id_locale")
                        .Where("translations.id_string", idString)
                        .Select(
                            "translations.id",
                            "translations.translated",
                            "locales.id as locale_id",
                            "locales.name_text as locale_name")
                        .OrderBy("locales.name_text", "translations.translated");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var translations = await dbConnection.QueryAsync<TranslationDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return translations;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllTranslationsInStringWithLocaleByID)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(TranslationRepository)}.{nameof(TranslationRepository.GetAllTranslationsInStringWithLocaleByID)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Обновление поля translated.
        /// </summary>
        /// <param name="translations"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTranslatedAsync(IEnumerable<TranslationDTO> translations)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    foreach (var translation in translations)
                    {
                        var query = new Query("translations")
                            .Where("id", translation.id)
                            .AsUpdate(new { translation.translated });

                        var compiledQuery = _compiler.Compile(query);
                        LogQuery(compiledQuery);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings
                        );

                    }

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.UpdateSubstringToTranslateAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.UpdateSubstringToTranslateAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }











        /// <summary>
        /// Получает записи из определенного и открытых проектов
        /// </summary>
        /// <param name="fileId">id определенного проекта</param>
        /// <returns></returns>
        public IEnumerable<TranslationSubstring> GetStringsInVisibleAndCurrentProjectd(Guid projectId)
        {

            var query = "SELECT TS.id, " +
                "TS.substring_to_translate " +

                "FROM translation_substrings AS TS " +
                "INNER JOIN files AS F ON TS.id_file_owner = F.id " +
                "INNER JOIN localization_projects AS LP ON LP.id= F.id_localization_project " +

                "where LP.id =@Id ";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = projectId };
                    this.LogQuery(query, param);
                    IEnumerable<TranslationSubstring> strings = dbConnection.Query<TranslationSubstring>(query, param);
                    return strings;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetStringsInVisibleAndCurrentProjectdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetStringsInVisibleAndCurrentProjectdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }


        /// <summary>
        /// Метод получения всех по id_locale
        /// </summary>
        /// <param name="id_locale">id языка, варианты перевода которой необходимы</param>
        /// <returns>Список вариантов перевода</returns>
        public IEnumerable<Translation> GetAllTranslationsByID_locale(Guid id_locale)
        {
            var query = "SELECT t.id_string, " +
                        "t.translated, " +
                        "t.confirmed, " +
                        "t.id_user, " +
                        "t.datetime, " +
                        "t.id_locale, " +
                        "t.selected, " +
                        "t.id, " +
                        "u.name_text AS User_Name " +
                        "FROM translations as t " +
                        "INNER JOIN users AS u ON u.id = t.id_user " +
                        "WHERE id_locale = @id_locale";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = id_locale };
                    this.LogQuery(query, param);
                    IEnumerable<Translation> translations = dbConnection.Query<Translation>(query, param);
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
        /// Метод получения всех вариантов перевода конкретной фразы
        /// </summary>
        /// <param name="idString">id фразы, варианты перевода которой необходимы</param>
        /// <returns>Список вариантов перевода</returns>
        public IEnumerable<Translation> GetAllTranslationsInStringWithLocale(Guid idString)
        {
            var query = "SELECT t.id_string, " +
                        "t.translated, " +
                        "t.confirmed, " +
                        "t.id_user, " +
                        "t.datetime, " +
                        "t.id_locale, " +
                        "t.selected, " +
                        "t.id, " +
                        "u.name_text AS User_Name " +
                        "FROM translations as t " +
                        "INNER JOIN users AS u ON u.id = t.id_user " +
                        "WHERE id_string = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = idString };
                    this.LogQuery(query, param);
                    IEnumerable<Translation> translations = dbConnection.Query<Translation>(query, param);
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



    }
}
