using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Models.Parser;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class FilesRepository : BaseRepository, IFilesRepository
    {

        private readonly string _insertFileSql =
            "INSERT INTO files (" +
            "id_localization_project, " +
            "name_text, " +
            "description, " +
            "date_of_change, " +
            "strings_count, " +
            "version, " +
            "priority, " +
            "id_folder_owner, " +
            "encod, " +
            "is_folder, " +
            "original_full_text, " +
            "is_last_version, " +
            "id_previous_version, " +
            "translator_name, " +
            "download_name," +
            "visibility" +
            ") "
            + "VALUES (" +
            "@ID_Localization_Project," +
            "@Name_text, " +
            "@Description, " +
            "@Date_Of_Change, " +
            "@Strings_Count, " +
            "@Version, " +
            "@Priority, " +
            "@ID_Folder_Owner, " +
            "@Encod, " +
            "@Is_Folder, " +
            "@Original_Full_Text, " +
            "@Is_Last_Version, " +
            "@Id_Previous_Version, " +
            "@Translator_Name, " +
            "@Download_Name," +
            "@visibility)";

        private UserActionRepository _action;
        private TranslationSubstringRepository _tsr;

        public FilesRepository(string connectionStr) : base(connectionStr)
        {
            _action = new UserActionRepository(connectionStr);
            _tsr = new TranslationSubstringRepository(connectionStr);
        }

        public async Task<IEnumerable<File>> GetAllAsync(int? userId, int? projectId)
        {
            var sqlString = @"SELECT f.*
            FROM files as f
            inner join  localization_projects as lp
            on f.id_localization_project = lp.id
            inner join participants as p

            on lp.id = p.id_localization_project
            where visibility = true and lp.id = " + (int)projectId + " and p.id_user = " + (int)userId;
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlString);
                    return await connection.QueryAsync<File>(sqlString);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<File> GetByIDAsync(int id)
        {
            var sqlString = "SELECT * FROM files WHERE id = @id";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(sqlString, param);
                    return await connection.QuerySingleOrDefaultAsync<File>(sqlString, param);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<File> GetByIDAsync(int id, int? conditionsId)
        {

            ///TODO нужно условий проверять
            var sqlString = "SELECT * FROM files WHERE id = @id";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(sqlString, param);
                    return await connection.QuerySingleOrDefaultAsync<File>(sqlString, param);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<File> GetLastVersionByNameAndParentIdAsync(string name, int? parentId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("files")
                        .Where("id_folder_owner", parentId)
                        .WhereLike("name_text", name)
                        .Where("is_last_version", true);

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    return await connection.QuerySingleOrDefaultAsync<File>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetLastVersionByNameAndParentIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetLastVersionByNameAndParentIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        //Нужно для формирования отчетов
        public IEnumerable<File> GetInitialFolders(int projectId)
        {
            var sqlString =
                $"SELECT * FROM files WHERE id_localization_project = @projectId AND id_folder_owner IS NULL " +
                "AND is_last_version=true";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var param = new { projectId };
                    this.LogQuery(sqlString, param);
                    return connection.Query<File>(sqlString, param);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetInitialFolders)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetInitialFolders)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<int> AddAsync(File file)
        {
            var sqlString = this._insertFileSql + " RETURNING id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                this.LogQuery(sqlString, param: file);
                return await connection.ExecuteScalarAsync<int>(sqlString, file);
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {


            var sqlStringTrSubLoc = @"DELETE FROM translation_substrings_locales as t
                        where t.id_translation_substrings in (
                            select ts.id
                            from public.translation_substrings as ts
            WHERE id_file_owner= " + id + ")";

            var sqlStringTrSub = @"DELETE FROM public.translation_substrings as ts
            WHERE id_file_owner = " + id;

            var sqlString = "DELETE FROM files WHERE id = " + id;
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {

                        this.LogQuery(sqlStringTrSubLoc);
                        connection.Execute(sqlStringTrSubLoc, transaction: transaction);

                        this.LogQuery(sqlStringTrSub);
                        connection.Execute(sqlStringTrSub, transaction: transaction);

                        var param = new { id };
                        this.LogQuery(sqlString, param);
                        var deletedRows = connection.Execute(sqlString, transaction: transaction);
                        transaction.Commit();

                        // Return result "deleted rows count more than 0"
                        return deletedRows > 0;
                    }
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.RemoveAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.RemoveAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(File file)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("files")
                        .AsUpdate(file)
                        .Where("id", file.id);
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var updatedRows = await connection.ExecuteAsync(compiledQuery.Sql, compiledQuery.NamedBindings);

                    // Return "updated rows count more than 0" result
                    return updatedRows > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.UpdateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<bool> UploadAsync(File file, IEnumerable<Locale> locales)
        {
            var sqlString = this._insertFileSql + " RETURNING id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        this.LogQuery(sqlString, file.GetType(), file);
                        var insertedId = await connection.ExecuteScalarAsync<int?>(sqlString, file, transaction);
                        if (!insertedId.HasValue)
                        {
                            this._loggerError.WriteLn("Не удалось загрузить файл в базу");
                            transaction.Rollback();
                            return false;
                        }

                        file.id = insertedId.Value;

                        if (file.is_folder)
                        {
                            /*Логирование*/
                            _action.AddAddFileActionAsync(file, file.id, WorkTypes.AddFile);
                            /**/
                            transaction.Commit();
                            return true;
                        }


                        var translationSubstrings = new Parser().Parse(file);
                        var translationSubstringsCount = translationSubstrings.Count;
                        var n = translationSubstringsCount;
                        foreach (var translationSubstring in translationSubstrings)
                        {
                            this.LogQuery(sqlString, translationSubstring.GetType(), translationSubstring);
                            //n -= await connection.ExecuteAsync(sqlString, translationSubstring, transaction);
                            //var idOfInsertedRow = await connection.ExecuteScalarAsync<int>(sqlString, translationSubstring, transaction);

                            var idOfInsertedRow = _tsr.AddAsync(translationSubstring, connection, transaction);
                            if (idOfInsertedRow != null)
                            {
                                n--;
                                //Мапим языки
                                List<int> newLoc = new List<int>();
                                foreach (var lc in locales)
                                {
                                    newLoc.Add(lc.id);
                                }

                                // _tsr.AddTranslationLocalesAsync(idOfInsertedRow, newLoc);
                                _tsr.AddTranslationLocalesTransactAsync(idOfInsertedRow, newLoc, connection, transaction);
                            }
                        }

                        if (n == 0)
                        {
                            file.strings_count = translationSubstringsCount;
                            sqlString = "UPDATE files SET strings_count = @Strings_Count WHERE id = @Id";
                            this.LogQuery(sqlString, file.GetType(), file);
                            await connection.ExecuteAsync(sqlString, file, transaction);
                        }

                        transaction.Commit();
                        return n == 0;
                    }
                    catch (NpgsqlException exception)
                    {
                        this._loggerError.WriteLn(
                            $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.UploadAsync)} {nameof(NpgsqlException)} ",
                            exception);
                        transaction.Rollback();
                        return false;
                    }
                    catch (ParserException exception)
                    {
                        this._loggerError.WriteLn("Ошибка в блоке распарсивания: ", exception);
                        //здесь фронтенд создает новый объект Parser и с помощью функции UseAllParsers получает Dictonary со всевозможными вариантами распарсивания
                        //ошибка возникает (пока) только в двух случаях: файл имеет неподдерживаемое системой расширение или внутри него не обнаружено строк для перевода
                        transaction.Rollback();
                        throw;
                    }
                    catch (Exception exception)
                    {
                        this._loggerError.WriteLn(
                            $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.UploadAsync)} {nameof(Exception)} ",
                            exception);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public async Task<string> GetFileContentAsync(int id, int id_locale = -1)
        {
            var sqlFileQuery = "SELECT * FROM files WHERE id = @id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var param = new { id };
                    this.LogQuery(sqlFileQuery, param);
                    var file = await connection.QuerySingleOrDefaultAsync<File>(sqlFileQuery, param);
                    if (id_locale == -1)
                    {
                        return file.original_full_text;
                    }
                    else
                    {
                        var sqlLocalizationProjectQuery =
                            "SELECT * FROM localization_projects WHERE id = @ID_LocalizationProject";
                        var localizationProject =
                            await connection.QuerySingleOrDefaultAsync<LocalizationProject>(sqlLocalizationProjectQuery,
                                new { ID = file.id });
                        var sqlTranslationSubstringsQuery =
                            "SELECT * FROM translation_substring WHERE id_file_owner = @id";
                        var translationSubstrings =
                            (await connection.QueryAsync<TranslationSubstring>(sqlTranslationSubstringsQuery, new { id }))
                            .AsList();
                        translationSubstrings.Sort((x, y) => x.position_in_text.CompareTo(y.position_in_text));
                        var output = file.original_full_text;
                        for (int i = translationSubstrings.Count - 1; i >= 0; i--)
                        {
                            var sqlTranslationQuery = string.Format(
                                "SELECT * FROM translations WHERE id_string = @id_translationSubstring AND id_locale = @id_locale{0} SORT BY selected DESC, confirmed DESC, datetime DESC LIMIT 1",
                                localizationProject.export_only_approved_translations ? " AND confirmed = true" : "");
                            var translation =
                                await connection.QuerySingleOrDefaultAsync<Translation>(sqlTranslationQuery,
                                    new { ID = translationSubstrings[i].id, id_locale });
                            if (translation == null)
                            {
                                if (localizationProject.AbleTo_Left_Errors)
                                {
                                    int n = translationSubstrings[i].position_in_text;
                                    while (n != 0 && (output[n - 1] != '\n' || output[n - 1] != '\r')) n--;
                                    int m = output.IndexOfAny(new char[] { '\r', '\n' },
                                        translationSubstrings[i].position_in_text);
                                    while (m != output.Length - 1 && output[m + 1] != '\n') m++;
                                    if (n > (i == 0 ? -1 : translationSubstrings[i - 1].position_in_text) &&
                                        m < (i == translationSubstrings.Count - 1
                                            ? output.Length
                                            : translationSubstrings[i + 1].position_in_text))
                                        output = output.Remove(n, m - n + 1);
                                    continue;
                                }

                                if (localizationProject.original_if_string_is_not_translated) continue;
                            }

                            output = output
                                .Remove(translationSubstrings[i].position_in_text,
                                    translationSubstrings[i].value.Length).Insert(
                                    translationSubstrings[i].position_in_text,
                                    translation == null ? localizationProject.Default_String : translation.Translated);
                        }

                        return output;
                    }
                }
                catch (NpgsqlException exception)
                {
                    this._loggerError.WriteLn("Ошибка в Upload NpgsqlException ", exception);
                    return null;
                }
                catch (Exception exception)
                {
                    this._loggerError.WriteLn(
                        $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.UploadAsync)} Exception ",
                        exception);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<File>> GetByProjectIdAsync(int projectId, string fileNamesSearch = null)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query = new Query("files")
                    .Where("id_localization_project", projectId)
                    .Where("is_last_version", true)
                    .Where("visibility", true);

                if (!string.IsNullOrEmpty(fileNamesSearch))
                {
                    var fileNamesSearchPattern = $"%{fileNamesSearch}%";
                    query = query.WhereLike("name_text", fileNamesSearchPattern);
                }

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                return await dbConnection.QueryAsync<File>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                );
            }
        }

        public async Task ChangeParentFolderAsync(int fileId, int? newParentId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query = new Query("files")
                    .Where("id", fileId)
                    .AsUpdate(new[] { "id_folder_owner" }, new object[] { newParentId });

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                await dbConnection.ExecuteAsync(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                );
            }
        }

        public async Task AddTranslationLocalesAsync(int fileId, IEnumerable<int> localesIds)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                dbConnection.Open();
                using (IDbTransaction transaction = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        foreach (var localeId in localesIds)
                        {
                            var sql =
                                "INSERT INTO files_locales " +
                                "(" +
                                "id_file, " +
                                "id_locale, " +
                                "percent_of_confirmed, " +
                                "percent_of_translation" +
                                ") VALUES " +
                                "(" +
                                "@ID_File, " +
                                "@ID_Locale, " +
                                "0, " +
                                "0" +
                                ")";
                            var param = new { ID_File = fileId, ID_Locale = localeId };
                            this.LogQuery(sql, param);

                            await dbConnection.ExecuteAsync(
                                sql: sql,
                                param: param, transaction: transaction);
                        }

                        //вставить обновление локалей

                        // не знаю чей это метод, но я добавил вторым параметром id языка для перевода, если нужно, то применяй
                        var strings = _tsr.GetStringsByFileIdAsync(fileId, null);
                        foreach (var str in strings.Result)
                        {
                            _tsr.AddTranslationLocalesTransactAsync(str.id, localesIds, dbConnection, transaction);
                        }

                    }
                    catch (NpgsqlException exception)
                    {
                        this._loggerError.WriteLn(
                            $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.AddTranslationLocalesAsync)} {nameof(NpgsqlException)} ",
                            exception);
                        transaction.Rollback();
                    }
                    catch (Exception exception)
                    {
                        this._loggerError.WriteLn(
                            $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.AddTranslationLocalesAsync)} {nameof(Exception)} ",
                            exception);
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<IEnumerable<Locale>> GetLocalesForFileAsync(int fileId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query =
                    new Query("locales")
                        .WhereIn("id",
                            new Query("files_locales")
                                .Select("id_locale")
                                .Where("id_file", fileId));

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                return await dbConnection.QueryAsync<Locale>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                );
            }
        }

        public async Task DeleteTranslationLocalesAsync(int fileId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query =
                    new Query("files_locales")
                        .Where("id_file", fileId)
                        .AsDelete();

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                await dbConnection.ExecuteAsync(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);
            }
        }

        public async Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoByIdAsync(int fileId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query =
                    new Query("files_locales")
                        .Select(
                            "id_locale as LocaleId",
                            "percent_of_translation as PercentOfTranslation",
                            "percent_of_confirmed as PercentOfConfirmed"
                        )
                        .Where("id_file", fileId);

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                return await dbConnection.QueryAsync<FileTranslationInfo>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);
            }
        }

        public async Task<IEnumerable<File>> GetFilesByParentFolderIdAsync(int parentFolderId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query =
                    new Query("files")
                        .Where("id_folder_owner", parentFolderId)
                        .Where("is_last_version", true)
                        .Where("visibility", true);

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                return await dbConnection.QueryAsync<File>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);
            }
        }


    }
}
