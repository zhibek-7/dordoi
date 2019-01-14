using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Models.Parser;
using Npgsql;
using SqlKata;
using Utilities.Logs;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class FilesRepository : BaseRepository, IFilesRepository
    {
        private readonly string connectionString;
        private ILogTools _log;

        public FilesRepository(ITranslationSubstringRepository translationSubstringRepository)
        {
            //TODO потом нужно переделать. Не должно быть статика
            connectionString = PostgreSqlNativeContext.getInstance().ConnectionString;
            _log = ExceptionLog.GetLog();
        }

        public FilesRepository(string connectionString)
        {
            this.connectionString = connectionString;
            _log = ExceptionLog.GetLog();
        }

        public async Task<IEnumerable<File>> GetAllAsync()
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM \"Files\"";

            try
            {
                // Using new posgresql connection
                using (var connection = new NpgsqlConnection(connectionString))
                {

                    Task<IEnumerable<File>> quer = connection.QueryAsync<File>(sqlString);
                    // Execute select query and return enumerable of file objects
                    return await quer;
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                _log.WriteLn("Ошибка в GetAll NpgsqlException ", exception);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                _log.WriteLn("Ошибка в GetAll exception ", exception);

                return null;
            }
        }

        public async Task<File> GetByIDAsync(int id)
        {
            // Sql string to select row by id
            var sqlString = "SELECT * FROM \"Files\" WHERE \"ID\" = @id";

            try
            {
                // Using new posgresql connection
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    // Execute select query and return file object
                    return await connection.QuerySingleOrDefaultAsync<File>(sqlString, new { id });
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                _log.WriteLn("Ошибка в GetByID NpgsqlException ", exception);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                _log.WriteLn("Ошибка в GetByID exception ", exception);

                return null;
            }
        }

        public async Task<File> GetByNameAndParentId(string name, int? parentId)
        {
            var parentExp = parentId.HasValue ? "\"ID_FolderOwner\" = @parentId" : "\"ID_FolderOwner\" IS NULL";

            // Sql string to select row by name and parent id
            var sqlString = $"SELECT * FROM \"Files\" WHERE \"Name\" LIKE @name AND {parentExp}";

            try
            {
                // Using new posgresql connection
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    // Execute select query and return file object
                    return await connection.QuerySingleOrDefaultAsync<File>(sqlString, new { name, parentId });
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                _log.WriteLn("Ошибка в GetByNameAndParentId NpgsqlException ", exception);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                _log.WriteLn("Ошибка в GetByNameAndParentId exception ", exception);

                return null;
            }
        }

        //Нужно для формирования отчетов
        public IEnumerable<File> GetInitialFolders(int projectId)
        {
            var sqlString = $"SELECT * FROM \"Files\" WHERE \"ID_LocalizationProject\" = @projectId AND \"ID_FolderOwner\" IS NULL";

            try
            {
                // Using new posgresql connection
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    // Execute select query and return enumerable of file objects
                    return connection.Query<File>(sqlString, new { projectId });
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                _log.WriteLn("Ошибка в GetInitialFolders NpgsqlException ", exception);

                return null;
            }
            catch (Exception exception)
            {
                _log.WriteLn("Ошибка в GetInitialFolders exception ", exception);

                return null;
            }
        }

        public async Task<int> AddAsync(File file)
        {
            // Sql string to insert query
            var sqlString = "INSERT INTO \"Files\" (\"Name\", \"Description\", \"DateOfChange\", " +
                            "\"StringsCount\", \"Version\", \"Priority\", \"Encoding\", \"OriginalFullText\", " +
                            "\"IsFolder\", \"ID_LocalizationProject\", \"ID_FolderOwner\") " +
                            "VALUES (@Name, @Description, @DateOfChange, @StringsCount, @Version, " +
                            "@Priority, @Encoding, @OriginalFullText, @IsFolder, @ID_LocalizationProject, " +
                            "@ID_FolderOwner)";

            try
            {
                // Using new posgresql connection
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    // Execute insert query and return count of inserted rows
                    return await connection.ExecuteAsync(sqlString, file);
                }
            }
            catch (NpgsqlException exception)
            {
                _log.WriteLn("Ошибка в Add NpgsqlException ", exception);

                return 0;
            }
            catch (Exception exception)
            {
                _log.WriteLn("Ошибка в Add Exception ", exception);

                return 0;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            // Sql string for delete query
            var sqlString = "DELETE FROM \"Files\" WHERE \"ID\" = @id";

            try
            {
                // Using new posgresql connection
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    // Execute delete query
                    var deletedRows = await connection.ExecuteAsync(sqlString, new { id });

                    // Return result "deleted rows count more than 0"
                    return deletedRows > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                _log.WriteLn("Ошибка в Remove NpgsqlException ", exception);

                return false;
            }
            catch (Exception exception)
            {
                _log.WriteLn("Ошибка в Remove exception ", exception);

                return false;
            }
        }

        public async Task<bool> UpdateAsync(File file)
        {
            // Sql string for update query
            var sqlString = "UPDATE \"Files\" SET \"Name\" = @Name, \"DateOfChange\" = @DateOfChange, " +
                            "\"StringsCount\" = @StringsCount, \"Encoding\" = @Encoding, " +
                            "\"ID_FolderOwner\" = @ID_FolderOwner, \"OriginalFullText\" = @OriginalFullText, " +
                            "\"IsFolder\" = @IsFolder WHERE \"ID\" = @Id";

            try
            {
                // Using new posgresql connection
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    // Execute update query
                    var updatedRows = await connection.ExecuteAsync(sqlString, file);

                    // Return "updated rows count more than 0" result
                    return updatedRows > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                _log.WriteLn("Ошибка в Update NpgsqlException ", exception);

                return false;
            }
            catch (Exception exception)
            {
                _log.WriteLn("Ошибка в Update exception ", exception);

                return false;
            }
        }

        public async Task<bool> Upload(File file)
        {
            var sqlString = "INSERT INTO \"Files\" (" +
                            "\"ID_LocalizationProject\", " +
                            "\"Name\", " +
                            "\"Description\", " +
                            "\"DateOfChange\", " +
                            "\"StringsCount\", " +
                            "\"ID_FolderOwner\", " +
                            "\"Encoding\", " +
                            "\"IsFolder\", " +
                            "\"OriginalFullText\"" +
                            ") " +
                            "VALUES (" +
                            "@ID_LocalizationProject," +
                            "@Name, " +
                            "@Description, " +
                            "@DateOfChange, " +
                            "@StringsCount, " +
                            "@ID_FolderOwner, " +
                            "@Encoding, " +
                            "@IsFolder, " +
                            "@OriginalFullText" +
                            ") " +
                            "RETURNING \"ID\"";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
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
                                n -= await connection.ExecuteAsync(sqlString, translationSubstring, transaction);
                            }
                            if (n == 0)
                            {
                                file.StringsCount = translationSubstringsCount;
                                sqlString = "UPDATE \"Files\" SET \"StringsCount\" = @StringsCount WHERE \"ID\" = @Id";
                                await connection.ExecuteAsync(sqlString, file, transaction);
                            }
                            transaction.Commit();
                            return n == 0;
                        }
                    }
                    catch (NpgsqlException exception)
                    {
                        this._loggerError.WriteLn("Ошибка в Upload NpgsqlException ", exception);
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
                        this._loggerError.WriteLn($"Ошибка в {nameof(FilesRepository)}.{nameof(FilesRepository.Upload)} Exception ", exception);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public async Task<IEnumerable<File>> GetByProjectIdAsync(int projectId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query = new Query("Files")
                    .Where("ID_LocalizationProject", projectId);
                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                dbConnection.Open();
                var files = await dbConnection.QueryAsync<File>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
                dbConnection.Close();
                return files;
            }
        }

    }
}
