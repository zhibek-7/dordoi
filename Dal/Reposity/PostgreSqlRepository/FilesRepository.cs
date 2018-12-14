using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Models.DatabaseEntities;
using Npgsql;
using Utilities.Logs;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class FilesRepository : IFilesRepository
    {
        private readonly string connectionString;
        private ILogTools _log;

        public FilesRepository(string connectionString)
        {
            this.connectionString = connectionString;
            _log = ExceptionLog.GetLog();
        }

        public async Task<IEnumerable<File>> GetAll()
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

        public async Task<File> GetByID(int id)
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
                    return await connection.QuerySingleAsync<File>(sqlString, new { name, parentId });
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

        public async Task<int> Add(File file)
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

        public async Task<bool> Remove(int id)
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

        public async Task<bool> Update(File file)
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
    }
}
