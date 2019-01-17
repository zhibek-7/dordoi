using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
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
            "INSERT INTO \"Files\" (" +
            "\"ID_LocalizationProject\", " +
            "\"Name\", " +
            "\"Description\", " +
            "\"DateOfChange\", " +
            "\"StringsCount\", " +
            "\"Version\", " +
            "\"Priority\", " +
            "\"ID_FolderOwner\", " +
            "\"Encoding\", " +
            "\"IsFolder\", " +
            "\"OriginalFullText\", " +
            "\"IsLastVersion\"" +
            ") " +
            "VALUES (" +
            "@ID_LocalizationProject," +
            "@Name, " +
            "@Description, " +
            "@DateOfChange, " +
            "@StringsCount, " +
            "@Version, " +
            "@Priority, " +
            "@ID_FolderOwner, " +
            "@Encoding, " +
            "@IsFolder, " +
            "@OriginalFullText, " +
            "@IsLastVersion" +
            ")";

        private readonly string connectionString;

        public FilesRepository()
        {
            //TODO потом нужно переделать. Не должно быть статика
            connectionString = PostgreSqlNativeContext.getInstance().ConnectionString;
        }

        public FilesRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<File>> GetAllAsync()
        {
            var sqlString = "SELECT * FROM \"Files\"";
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
            var sqlString = "SELECT * FROM \"Files\" WHERE \"ID\" = @id";
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

        public async Task<File> GetLastVersionByNameAndParentId(string name, int? parentId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Files")
                        .Where("ID_FolderOwner", parentId)
                        .WhereLike("Name", name)
                        .Where("IsLastVersion", true);

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
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetLastVersionByNameAndParentId)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.GetLastVersionByNameAndParentId)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        //Нужно для формирования отчетов
        public IEnumerable<File> GetInitialFolders(int projectId)
        {
            var sqlString = $"SELECT * FROM \"Files\" WHERE \"ID_LocalizationProject\" = @projectId AND \"ID_FolderOwner\" IS NULL";
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
            var sqlString = this._insertFileSql + " RETURNING \"ID\"";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                this.LogQuery(sqlString, param: file);
                return await connection.ExecuteScalarAsync<int>(sqlString, file);
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var sqlString = "DELETE FROM \"Files\" WHERE \"ID\" = @id";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(sqlString, param);
                    var deletedRows = await connection.ExecuteAsync(sqlString, param);

                    // Return result "deleted rows count more than 0"
                    return deletedRows > 0;
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
                    var query = new Query("Files")
                        .AsUpdate(file)
                        .Where("ID", file.ID);
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

        public async Task<bool> Upload(File file)
        {
            var sqlString = this._insertFileSql + " RETURNING \"ID\"";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        this.LogQuery(sqlString, param: file);
                        var insertedId = await connection.ExecuteScalarAsync<int?>(sqlString, file, transaction);
                        if (!insertedId.HasValue)
                        {
                            this._loggerError.WriteLn("Insertion into files didn't return id.");
                            transaction.Rollback();
                            return false;
                        }
                        file.ID = insertedId.Value;

                        if (file.IsFolder)
                        {
                            transaction.Commit();
                            return true;
                        }

                        sqlString = "INSERT INTO \"TranslationSubstrings\" " +
                                    "(" +
                                    "\"SubstringToTranslate\", " +
                                    "\"Context\", " +
                                    "\"ID_FileOwner\", " +
                                    "\"Value\", " +
                                    "\"PositionInText\"" +
                                    ") " +
                                    "VALUES (" +
                                    "@SubstringToTranslate, " +
                                    "@Context, " +
                                    "@ID_FileOwner, " +
                                    "@Value, " +
                                    "@PositionInText" +
                                    ")";
                        using (var parser = new Parser())
                        {
                            var translationSubstrings = parser.Parse(file);
                            var translationSubstringsCount = translationSubstrings.Count;
                            var n = translationSubstringsCount;
                            foreach (var translationSubstring in translationSubstrings)
                            {
                                this.LogQuery(sqlString, param: translationSubstring);
                                n -= await connection.ExecuteAsync(sqlString, translationSubstring, transaction);
                            }
                            if (n == 0)
                            {
                                file.StringsCount = translationSubstringsCount;
                                sqlString = "UPDATE \"Files\" SET \"StringsCount\" = @StringsCount WHERE \"ID\" = @Id";
                                this.LogQuery(sqlString, param: file);
                                await connection.ExecuteAsync(sqlString, file, transaction);
                            }
                            transaction.Commit();
                            return n == 0;
                        }
                    }
                    catch (NpgsqlException exception)
                    {
                        this._loggerError.WriteLn(
                            $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.Upload)} {nameof(NpgsqlException)} ",
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
                            $"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.Upload)} {nameof(Exception)} ",
                            exception);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public async Task<File> Load(int id, int id_locale = -1)
        {
            var sqlFileQuery = "SELECT * FROM \"Files\" WHERE \"ID\" = @id";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var file = await connection.QuerySingleOrDefaultAsync<File>(sqlFileQuery, new { id });
                    if (id_locale != -1)
                    {
                        var sqlLocalizationProjectQuery = "SELECT * FROM \"LocalizationProjects\" WHERE \"ID\" = @ID_LocalizationProject";
                        var localizationProject = await connection.QuerySingleOrDefaultAsync<LocalizationProject>(sqlLocalizationProjectQuery, new { file.ID });
                        var sqlTranslationSubstringsQuery = "SELECT * FROM \"TranslationSubstring\" WHERE \"ID_FileOwner\" = @id";
                        var translationSubstrings = (await connection.QueryAsync<TranslationSubstring>(sqlTranslationSubstringsQuery, new { id })).AsList();
                        translationSubstrings.Sort((x, y) => x.PositionInText.CompareTo(y.PositionInText));
                        var output = file.OriginalFullText;
                        for (int i = translationSubstrings.Count - 1; i >= 0; i--)
                        {
                            var sqlTranslationQuery = string.Format("SELECT * FROM \"Translations\" WHERE \"ID_String\" = @id_translationSubstring AND \"ID_Locale\" = @id_locale{0} SORT BY \"Selected\" DESC, \"Confirmed\" DESC, \"DateTime\" DESC LIMIT 1", localizationProject.export_only_approved_translations ? " AND \"Confirmed\" = true" : "");
                            var translation = await connection.QuerySingleOrDefaultAsync<Translation>(sqlTranslationQuery, new { translationSubstrings[i].ID, id_locale });
                            if (translation == null && localizationProject.original_if_string_is_not_translated) continue;
                            output = output.Remove(translationSubstrings[i].PositionInText, translationSubstrings[i].Value.Length).Insert(translationSubstrings[i].PositionInText, translation == null ? localizationProject.DefaultString : translation.Translated);
                        }
                        //how to send output file to front-end?
                    }
                    else
                    {
                        //using (var sw = new System.IO.StreamWriter(System.IO.File.Open("NEED_filePath", System.IO.FileMode.CreateNew), Encoding.GetEncoding(file.Encoding)))
                        //{
                        //    sw.Write(file.OriginalFullText);
                        //}
                        //how to send original file to front-end ?
                    }
                    return null;
                }
                catch (NpgsqlException exception)
                {
                    this._loggerError.WriteLn("Ошибка в Upload NpgsqlException ", exception);
                    return null;
                }
                catch (Exception exception)
                {
                    this._loggerError.WriteLn($"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.Upload)} Exception ", exception);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<File>> GetByProjectIdAsync(int projectId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query = new Query("Files")
                    .Where("ID_LocalizationProject", projectId)
                    .Where("IsLastVersion", true);
                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                return await dbConnection.QueryAsync<File>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
            }
        }

        public async Task<IEnumerable<File>> GetByProjectIdAsync(int projectId, string fileNamesSearch)
        {
            var fileNamesSearchPattern = $"%{fileNamesSearch}%";
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query = new Query("Files")
                    .Where("ID_LocalizationProject", projectId)
                    .Where("IsLastVersion", true)
                    .WhereLike("Name", fileNamesSearchPattern);

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                return await dbConnection.QueryAsync<File>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
            }
        }

    }
}
