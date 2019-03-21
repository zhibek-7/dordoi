using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using Models.DatabaseEntities.DTO;
using SqlKata;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class TranslationSubstringRepository : BaseRepository, ITranslationSubstringRepository
    {
        //private PostgreSqlNativeContext context;


        public TranslationSubstringRepository(string connectionString) : base(connectionString)
        {
        }

        /// <summary> 
        /// Добавляет новую фразу 
        /// </summary> 
        /// <param name="item">Новая фраза</param> 
        /// <returns>Кол-во добавленных фраз</returns> 
        public Task<Guid?> AddAsync(TranslationSubstring item)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    Guid? t = AddAsync(item, connection, transaction);
                    return Task.FromResult(t);
                }
            }
        }

        public Guid? AddAsync(TranslationSubstring translationSubstring, NpgsqlConnection connection, IDbTransaction transaction)
        {
            var sqlString = "INSERT INTO translation_substrings " +
                            "(" +
                            "substring_to_translate, " +
                            "context, " +
                            "id_file_owner, " +
                            "value, " +
                            "position_in_text" +
                            ") " +
                            "VALUES (" +
                            "@substring_to_translate, " +
                            "@context, " +
                            "@id_file_owner, " +
                            "@value, " +
                            "@position_in_text" +
                            ") RETURNING translation_substrings.id";
            try
            {
                this.LogQuery(sqlString, translationSubstring.GetType(), translationSubstring);
                var id = connection.ExecuteScalar(sqlString, translationSubstring, transaction);

                return Guid.Parse(id + "");
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                transaction.Rollback();
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                transaction.Rollback();
                return null;
            }
        }


        /// <summary>
        /// Получает все фразы
        /// </summary>
        /// <returns>Список фраз</returns>
        public async Task<IEnumerable<TranslationSubstring>> GetAllAsync()
        {
            var query = "SELECT * FROM translation_substrings";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query);

                    IEnumerable<TranslationSubstring> strings = await dbConnection.QueryAsync<Models.DatabaseEntities.TranslationSubstring>(query);
                    return strings;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

        /// <summary>
        /// Получает запись с конкретным id
        /// </summary>
        /// <param name="id">id необходимой фразы</param>
        /// <returns>Запись с необходимым id</returns>
        public async Task<TranslationSubstring> GetByIDAsync(Guid id)
        {
            var query = "SELECT * " +
                        "FROM translation_substrings " +
                        "WHERE id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = id };
                    this.LogQuery(query, param);
                    var foundedString = await dbConnection.QuerySingleAsync<Models.DatabaseEntities.TranslationSubstring>(query, param);
                    return foundedString;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }


        ///// <summary>
        ///// Фильтрует список строк по определенной фразе
        ///// </summary>
        ///// <param name="filtredString">фраза по которой происходит фильтрация</param>
        ///// <param name="filtredListOfStrings">список строк среди которых происходит фильтрация</param>
        ///// <returns>список строк содержащихся в списке строк </returns>
        //public async Task<IEnumerable<TranslationSubstring>> FilterByString(string filtredString, IEnumerable<TranslationSubstring> filtredListOfStrings)
        //{
        //    var query = "";
        //    try
        //    {
        //        using (var dbConnection = new NpgsqlConnection(connectionString))
        //        {
        //            this.LogQuery(query);
        //            IEnumerable<TranslationSubstring> filtredStrings = await dbConnection.QueryAsync<TranslationSubstring>(query);
        //            return filtredStrings;
        //        }
        //    }
        //    catch (NpgsqlException exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.FilterByString)} {nameof(NpgsqlException)} ",
        //            exception);
        //        return null;
        //    }
        //    catch (Exception exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.FilterByString)} {nameof(Exception)} ",
        //            exception);
        //        return null;
        //    }
        //}


        /// <summary>
        /// Получает записи из определенного и открытых проектов
        /// </summary>
        /// <param name="fileId">id определенного проекта</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationSubstring>> GetStringsInVisibleAndCurrentProjectdAsync(Guid projectId)
        {
            var query = "SELECT * " +
                        "FROM translation_substrings AS TS " +
                        "INNER JOIN files AS F ON TS.id_file_owner = F.id " +
                        "INNER JOIN localization_projects AS LP " +
                        "WHERE LP.id = @Id " +
                        "OR LP.visibility = true ";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = projectId };
                    this.LogQuery(query, param);
                    IEnumerable<TranslationSubstring> strings = await dbConnection.QueryAsync<TranslationSubstring>(query, param);
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
        /// Получает записи из определенного файла по id файла
        /// </summary>
        /// <param name="fileId">id файла</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(Guid fileId, Guid? localeId)
        {
            var queryForSubstingsInFileWithLocale = "SELECT TS.substring_to_translate AS substring_to_translate, " +
                        "TS.description AS description, " +
                        "TS.context AS context, " +
                        "TS.translation_max_length AS translation_max_length, " +
                        "TS.id_file_owner AS id_file_owner, " +
                        "TS.value AS value, " +
                        "TS.position_in_text AS position_in_text, " +
                        "TS.id AS id " +
                        "FROM translation_substrings AS TS " +
                        "INNER JOIN files AS F ON TS.id_file_owner = F.id " +
                        "INNER JOIN translation_substrings_locales AS TSL ON TSL.id_translation_substrings = TS.id " +
                        "WHERE F.id = @FileId AND TSL.id_locale = @LocaleId ";

            var queryForSubstingsInFile = "SELECT TS.substring_to_translate AS substring_to_translate, TS.description AS description, " +
                        "TS.context AS context, TS.translation_max_length AS translation_max_length," +
                        "TS.id_file_owner AS id_file_owner, TS.value AS value," +
                        "TS.position_in_text AS position_in_text, TS.id AS id " +
                        "FROM translation_substrings AS TS " +
                        "INNER JOIN files AS F ON TS.id_file_owner = F.id " +
                        "WHERE F.id = @FileId";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    IEnumerable<TranslationSubstring> stringsInFile;

                    if (localeId != null)
                    {
                        var param = new { FileId = fileId, LocaleId = localeId };
                        this.LogQuery(queryForSubstingsInFileWithLocale, param);
                        stringsInFile = await dbConnection.QueryAsync<TranslationSubstring>(queryForSubstingsInFileWithLocale, param);
                    }
                    else
                    {
                        var param = new { FileId = fileId };
                        this.LogQuery(queryForSubstingsInFile, param);
                        stringsInFile = await dbConnection.QueryAsync<TranslationSubstring>(queryForSubstingsInFile, param);
                    }

                    if (localeId != null)
                    {
                        foreach (var translationSubstring in stringsInFile)
                        {
                            translationSubstring.status = await GetStatusOfTranslationSubstringAsync(translationSubstring.id, localeId);
                        }
                    }
                    else
                    {
                        foreach (var translationSubstring in stringsInFile)
                        {
                            translationSubstring.status = await GetStatusOfTranslationSubstringAsync(translationSubstring.id, null);
                        }
                    }

                    return stringsInFile;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetStringsByFileIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetStringsByFileIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Получить изображения строки для перевода
        /// </summary>
        /// <param name="translationSubstringId">id Строки для перевода</param>
        /// <returns>Список изображений</returns>
        public async Task<IEnumerable<Image>> GetImagesOfTranslationSubstringAsync(Guid translationSubstringId)
        {
            var query = "SELECT " +
                        "Im.id," +
                        "Im.url," +
                        "Im.name_text," +
                        "Im.date_time_added," +
                        "Im.body," +
                        "Im.id_user " +
                        "FROM translation_substrings AS TS " +
                        "INNER JOIN strings_context_images AS SCI ON SCI.id_string = TS.id " +
                        "INNER JOIN images AS Im ON Im.id = SCI.id_image " +
                        "WHERE TS.id = @TranslationSubstringId";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { TranslationSubstringId = translationSubstringId };
                    this.LogQuery(query, param);
                    IEnumerable<Image> images = await dbConnection.QueryAsync<Image>(query, param);
                    return images;
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
        /// Добавить изображение к комментарию
        /// </summary>
        /// <param name="img">Изображение</param>
        /// <param name="translationSubstringId">Id строки для перевода</param>
        /// <returns></returns>
        public async Task<Guid?> UploadImageAsync(Image img, Guid translationSubstringId)
        {
            var query1 = "INSERT INTO images (name_text, id_user, body, date_time_added)" +
                        "VALUES (@Name_text,  @ID_User, @body, @Date_Time_Added) " +
                        "RETURNING  images.id";

            var query2 = "INSERT INTO strings_context_images (id_string, id_image)" +
                        "VALUES (@TranslationSubstringId,  @ImageId) ";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query1, img.GetType(), img);
                    var idOfInsertedImage = await dbConnection.ExecuteScalarAsync<Guid>(query1, img);

                    var t = new { TranslationSubstringId = translationSubstringId, ImageId = idOfInsertedImage };

                    this.LogQuery(query2, t);
                    await dbConnection.ExecuteScalarAsync(query2, t);
                    return idOfInsertedImage;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.UploadImageAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.UploadImageAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_substrings")
                        .Where("id", id)
                        .AsDelete();


                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);


                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                        );

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.RemoveAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.RemoveAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Models.DatabaseEntities.TranslationSubstring item)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_substrings")
                        .Where("id", item.id)
                        .AsUpdate(item);

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                             sql: compiledQuery.Sql,
                             param: compiledQuery.NamedBindings
                             );
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.UpdateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }

        }

        public static Dictionary<string, string> SortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "id" },
            { "substring_to_translate", "substring_to_translate" },
            { "description", "description" },
            { "context", "context" },
            { "translation_max_length", "translation_max_length" },
            { "id_file_owner", "id_file_owner" },
            { "value", "value" },
            { "positionin_text", "positionin_text" },
            { "outdated", "outdated" },
            { "translation_memories.name_text", "translation_memories.name_text"}
        };

        public async Task<IEnumerable<TranslationSubstring>> GetByProjectIdAsync(
            Guid projectId,
            int offset,
            int limit,
            Guid? fileId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            //TODO !!!!
            if (sortBy == null || !sortBy.Any())
            {
                sortBy = new[] { "id" };
            }

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetByProjectIdQuery(
                        projectId: projectId,
                        fileId: fileId,
                        searchString: searchString);

                    query = this.ApplyPagination(
                        query: query,
                        offset: offset,
                        limit: limit);

                    query = this.ApplySorting(
                        query: query,
                        columnNamesMappings: TranslationSubstringRepository.SortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);

                    var translationSubstrings = await dbConnection.QueryAsync<TranslationSubstring>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                        );
                    return translationSubstrings;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByProjectIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByProjectIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<int?> GetByProjectIdCountAsync(
            Guid projectId,
            Guid? fileId = null,
            string searchString = null)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetByProjectIdQuery(
                        projectId: projectId,
                        fileId: fileId,
                        searchString: searchString);
                    query = query.AsCount();


                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);

                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                        );
                    return count;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByProjectIdCountAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByProjectIdCountAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

        /// <summary>
        /// ????????????????
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fileId"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private Query GetByProjectIdQuery(
            Guid projectId,
            Guid? fileId = null,
            string searchString = null)
        {
            try
            {

                var query = new Query("translation_substrings").Join("files", "translation_substrings.id_file_owner", "files.id")
               .Select(
                   "translation_substrings.id",
                   "substring_to_translate",
                   "translation_substrings.description",
                   "context",
                   "translation_max_length",
                   "id_file_owner",
                   "value",
                   "position_in_text",
                   "outdated"
               );
                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);
                if (fileId != null)
                {
                    query = query.Where("id_file_owner", fileId);
                }
                else
                {
                    query = query
                        .WhereIn("id_file_owner",
                            new Query("files")
                                .Select("id")
                                .Where("id_localization_project", projectId)
                                );
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchPattern = $"%{searchString}%";
                    query = query.WhereLike("substring_to_translate", searchPattern);
                }

                query = query.Where("visibility", true);
                return query;
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByProjectIdQuery)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByProjectIdQuery)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<IEnumerable<Locale>> GetLocalesForStringAsync(Guid? translationSubstringId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query =
                        new Query("locales")
                        .WhereIn("id",
                            new Query("translation_substrings_locales")
                            .Select("id_locale")
                            .Where("id_translation_substrings", translationSubstringId));

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);

                    var locales = await dbConnection.QueryAsync<Locale>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                        );
                    return locales;
                }

            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetLocalesForStringAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetLocalesForStringAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task DeleteTranslationLocalesAsync(Guid translationSubstringId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query =
                        new Query("translation_substrings_locales")
                        .Where("id_translation_substrings", translationSubstringId)
                        .AsDelete();
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.DeleteTranslationLocalesAsync)} {nameof(NpgsqlException)} ",
                    exception);

            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.DeleteTranslationLocalesAsync)} {nameof(Exception)} ",
                    exception);

            }

        }

        public async Task AddTranslationLocalesAsync(Guid translationSubstringId, IEnumerable<Guid> localesIds)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    await AddTranslationLocalesTransactAsync(translationSubstringId, localesIds, dbConnection);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.AddTranslationLocalesAsync)} {nameof(NpgsqlException)} ",
                    exception);

            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.AddTranslationLocalesAsync)} {nameof(Exception)} ",
                    exception);
            }
        }

        public async Task AddTranslationLocalesTransactAsync(Guid? translationSubstringId, IEnumerable<Guid> localesIds,
            NpgsqlConnection dbConnection, IDbTransaction transaction = null)
        {
            //TODO нужно удаление локалей сделать.
            Task<IEnumerable<Locale>> assignedLoc = GetLocalesForStringAsync(translationSubstringId);

            List<Guid> idAssignetLoc = new List<Guid>();
            List<Guid> idAssignetLocCopy = new List<Guid>();
            foreach (var localeId in assignedLoc.Result)
            {
                idAssignetLoc.Add(localeId.id);
            }

            idAssignetLocCopy.AddRange(idAssignetLoc);

            foreach (var localeId in localesIds)
            {
                //Назначаем только назначенные локали
                if (idAssignetLoc.Contains((Guid)localeId) == false)
                {
                    var sql =
                        "INSERT INTO translation_substrings_locales " +
                        "(" +
                        "id_translation_substrings, " +
                        "id_locale" +
                        ") VALUES " +
                        "(" +
                        "@Id_TranslationSubStrings, " +
                        "@Id_Locales" +
                        ")";
                    var param = new { Id_TranslationSubStrings = translationSubstringId, Id_Locales = localeId };
                    this.LogQuery(sql, param);

                    if (transaction != null)
                    {
                        dbConnection.Execute(
                            sql: sql,
                            param: param, transaction: transaction);
                    }
                    else
                    {
                        await dbConnection.ExecuteAsync(
                            sql: sql,
                            param: param);
                    }

                    if (localeId != null)
                    {
                        idAssignetLocCopy.Remove((Guid)localeId);
                    }
                }
            }

            //Удаляем не назначенные локали.
            await DellTranslationLocalesTransact(translationSubstringId, dbConnection, transaction, idAssignetLocCopy);

        }

        private async Task DellTranslationLocalesTransact(Guid? translationSubstringId, NpgsqlConnection dbConnection,
            IDbTransaction transaction, List<Guid> idAssignetLocCopy)
        {
            foreach (var localeId in idAssignetLocCopy)
            {
                var sql = "DELETE FROM public.translation_substrings_locales WHERE id_translation_substrings=" +
                          translationSubstringId + " and id_locale = " + localeId;
                this.LogQuery(sql);

                if (transaction != null)
                {
                    dbConnection.Execute(
                        sql: sql,
                        transaction: transaction);
                }
                else
                {
                    await dbConnection.ExecuteAsync(
                        sql: sql);
                }
            }
        }

        public async Task<string> GetStatusOfTranslationSubstringAsync(Guid translationSubstringId, Guid? localeId)
        {
            var query = "";

            if (localeId != null)
            {
                query = "SELECT * " +
                        "FROM translations AS T " +
                        "WHERE T.id_string = @translationSubstringId AND T.id_locale = @localeId;";
            }
            else
            {
                query = "SELECT * " +
                        "FROM translations AS T " +
                        "WHERE T.id_string = @translationSubstringId;";
            }


            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { TranslationSubstringId = translationSubstringId, localeId };
                    this.LogQuery(query, param);
                    var translationsOfTheString = await dbConnection.QueryAsync<Translation>(query, param);

                    string status = "Empty";

                    foreach (var translation in translationsOfTheString)
                    {
                        if (translation.Selected == true)
                        {
                            status = "Selected";
                            break;
                        }
                        if (translation.Confirmed == true)
                        {
                            status = "Confirmed";
                        }
                    }

                    return status;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetStatusOfTranslationSubstringAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetStatusOfTranslationSubstringAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }


        public async Task SetStatusOfTranslationSubstringAsync(Guid translationSubstringId, string status)
        {
            var setStatusOfStringQuery =
                "UPDATE translation_substrings SET " +
                "status = @status " +
                "WHERE id = @translationSubstringId";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { translationSubstringId, status };
                    this.LogQuery(setStatusOfStringQuery, param);
                    var result = await dbConnection.QueryAsync<Translation>(setStatusOfStringQuery, param);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.SetStatusOfTranslationSubstringAsync)} {nameof(NpgsqlException)} ",
                    exception);

            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.SetStatusOfTranslationSubstringAsync)} {nameof(Exception)} ",
                    exception);
            }
        }


        /// <summary>
        /// Удаление всех строк связанных с памятью переводов.
        /// </summary>
        /// <param name="translationMemoryId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveByTranslationMemoryAsync(Guid translationMemoryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var queryIds = new Query("translation_substrings")
                        .Join("translation_memories_strings", "translation_memories_strings.id_string", "translation_substrings.id")
                        .Where("translation_memories_strings.id_translation_memory", translationMemoryId)
                        .Select("translation_substrings.id");

                    var queryDelete = new Query("translation_substrings")
                        .WhereIn("id", queryIds).AsDelete();

                    var compiledQuery = _compiler.Compile(queryDelete);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.RemoveByTranslationMemoryAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.RemoveByTranslationMemoryAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }


        /// <summary>
        /// Возвращает строки (со связанными объектами).
        /// </summary>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="searchString">Шаблон строки (поиск по substring_to_translate).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationSubstringTableViewDTO>> GetAllWithTranslationMemoryByProjectAsync(
            Guid projectId,
            int offset,
            int limit,
            Guid? translationMemoryId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            if (sortBy == null || !sortBy.Any())
            {
                sortBy = new[] { "id" };
            }

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = GetAllWithTranslationMemoryByProjectQuery(
                        projectId,
                        translationMemoryId,
                        searchString);

                    query = ApplyPagination(
                        query: query,
                        offset: offset,
                        limit: limit);

                    query = ApplySorting(
                        query: query,
                        columnNamesMappings: TranslationSubstringRepository.SortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    var temp = await dbConnection.QueryAsync<TranslationSubstringTableViewDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );

                    var translationSubstrings = temp.GroupBy(t => t.id).Select(t => new TranslationSubstringTableViewDTO
                    {
                        id = t.Key,
                        substring_to_translate = t.FirstOrDefault().substring_to_translate,
                        translation_memories_name = string.Join(", ", t.Select(x => x.translation_memories_name).Distinct().OrderBy(n => n))
                    });

                    return translationSubstrings;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllWithTranslationMemoryByProjectAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllWithTranslationMemoryByProjectAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает количество строк.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="searchString">Шаблон строки (поиск по substring_to_translate).</param>
        /// <returns></returns>
        public async Task<int?> GetAllWithTranslationMemoryByProjectCountAsync(
            Guid projectId,
            Guid? translationMemoryId = null,
            string searchString = null)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = GetAllWithTranslationMemoryByProjectQuery(
                        projectId,
                        translationMemoryId,
                        searchString);
                    query = query.Distinct().AsCount("translation_substrings.id");


                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );
                    return count;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllWithTranslationMemoryByProjectCountAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllWithTranslationMemoryByProjectCountAsync)} {nameof(Exception)} ", exception);
                return null;
            }

        }

        /// <summary>
        /// Возвращает запрос строк (со связанными объектами).
        /// </summary>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <param name="searchString">Шаблон строки (поиск по substring_to_translate).</param>
        /// <returns></returns>
        private Query GetAllWithTranslationMemoryByProjectQuery(
            Guid projectId,
            Guid? translationMemoryId = null,
            string searchString = null)
        {
            try
            {

                var query = new Query("translation_substrings")
                    .Join("translation_memories_strings", "translation_memories_strings.id_string", "translation_substrings.id")
                    .Join("translation_memories", "translation_memories.id", "translation_memories_strings.id_translation_memory")
                    .Join("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories.id")
                    .Select("translation_substrings.id", "translation_substrings.substring_to_translate", "translation_memories.name_text as translation_memories_name");

                var compiledQuery = _compiler.Compile(query);
                LogQuery(compiledQuery);
                if (translationMemoryId != null)
                {
                    query = query.Where("translation_memories.id", translationMemoryId);
                }
                else
                {
                    query = query.Where("localization_projects_translation_memories.id_localization_project", projectId);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchPattern = $"%{searchString}%";
                    query = query.WhereLike("substring_to_translate", searchPattern);
                }

                return query;
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllWithTranslationMemoryByProjectQuery)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetAllWithTranslationMemoryByProjectQuery)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Обновление поля substring_to_translate
        /// </summary>
        /// <param name="translationSubstring"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSubstringToTranslateAsync(TranslationSubstringTableViewDTO translationSubstring)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var forSave = new
                    {
                        translationSubstring.substring_to_translate
                    };
                    var query = new Query("translation_substrings")
                        .Where("id", translationSubstring.id)
                        .AsUpdate(forSave);

                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );
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
        /// Удаление строк.
        /// </summary>
        /// <param name="ids">Идентификаторы строк.</param>
        /// <returns></returns>
        public async Task<bool> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("translation_substrings")
                            .WhereIn("id", ids)
                            .AsDelete();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.DeleteTranslationLocalesAsync)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.DeleteTranslationLocalesAsync)} {nameof(Exception)} ", exception);
                return false;
            }
        }





        /// <summary>
        /// Получает записи из определенного и открытых проектов
        /// </summary>
        /// <param name="projectId">id определенного проекта</param>
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





    }
}
