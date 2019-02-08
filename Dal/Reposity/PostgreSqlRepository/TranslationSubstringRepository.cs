using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<TranslationSubstring> GetByIDAsync(int id)
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


        /// <summary>
        /// Фильтрует список строк по определенной фразе
        /// </summary>
        /// <param name="filtredString">фраза по которой происходит фильтрация</param>
        /// <param name="filtredListOfStrings">список строк среди которых происходит фильтрация</param>
        /// <returns>список строк содержащихся в списке строк </returns>
        public async Task<IEnumerable<TranslationSubstring>> FilterByString(string filtredString, IEnumerable<TranslationSubstring> filtredListOfStrings)
        {
            var query = "";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query);
                    IEnumerable<TranslationSubstring> filtredStrings = await dbConnection.QueryAsync<TranslationSubstring>(query);
                    return filtredStrings;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.FilterByString)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.FilterByString)} {nameof(Exception)} ",
                    exception);
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
        public async Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(int fileId)
        {
            var query = "SELECT TS.substring_to_translate AS substring_to_translate, TS.description AS description, " +
                        "TS.context AS context, TS.translation_max_length AS translation_max_length," +
                        "TS.id_file_owner AS id_file_owner, TS.value AS value," +
                        "TS.position_in_text AS position_in_text, TS.id AS id " +
                        "FROM translation_substrings AS TS " +
                        "INNER JOIN files AS F ON TS.id_file_owner = F.id " +
                        "WHERE F.id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = fileId };
                    this.LogQuery(query, param);
                    IEnumerable<TranslationSubstring> stringsInFile = await dbConnection.QueryAsync<TranslationSubstring>(query, param);
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
        public async Task<IEnumerable<Image>> GetImagesOfTranslationSubstringAsync(int translationSubstringId)
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
        public async Task<int> UploadImageAsync(Image img, int translationSubstringId)
        {
            var query1 = "INSERT INTO images (name_text, id_user, body, url)" +
                        "VALUES (@Name,  @ID_User, @Data, @url) " +
                        "RETURNING  images.id";

            var query2 = "INSERT INTO strings_context_images (id_string, id_image)" +
                        "VALUES (@TranslationSubstringId,  @ImageId) ";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query1, img.GetType(), img);
                    var idOfInsertedImage = await dbConnection.ExecuteScalarAsync<int>(query1, img);

                    var t = new { TranslationSubstringId = translationSubstringId, ImageId = idOfInsertedImage };

                    this.LogQuery(query2, t);
                    await dbConnection.ExecuteScalarAsync(query2, t);
                    return idOfInsertedImage;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        public async Task<bool> RemoveAsync(int id)
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
        };

        public async Task<IEnumerable<TranslationSubstring>> GetByProjectIdAsync(
            int projectId,
            int offset,
            int limit,
            int? fileId = null,
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

        public async Task<int> GetByProjectIdCountAsync(
            int projectId,
            int? fileId = null,
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
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationSubstringRepository)}.{nameof(TranslationSubstringRepository.GetByProjectIdCountAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
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
            int projectId,
            int? fileId = null,
            string searchString = null)
        {
            try
            {

                var query = new Query("translation_substrings")
               .Select(
                   "id",
                   "substring_to_translate",
                   "description",
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
                                .Where("id_localization_project", projectId));
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

        public async Task<IEnumerable<Locale>> GetLocalesForStringAsync(int translationSubstringId)
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

        public async Task DeleteTranslationLocalesAsync(int translationSubstringId)
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

        public async Task AddTranslationLocalesAsync(int translationSubstringId, IEnumerable<int> localesIds)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    foreach (var localeId in localesIds)
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
                        await dbConnection.ExecuteAsync(
                            sql: sql,
                            param: param);
                    }
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

    }
}
