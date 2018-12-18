﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Models.DatabaseEntities;
using Models.Models;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class FilesRepository : IFilesRepository
    {
        private readonly string connectionString;

        public FilesRepository(string connectionString)
        {
            this.connectionString = connectionString;
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
                    // Execute select query and return enumerable of file objects
                    return await connection.QueryAsync<File>(sqlString);
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

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
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

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
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

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
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

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
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return 0;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

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
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return false;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

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
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return false;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

                return false;
            }
        }

        public async Task<bool> Upload(File file)
        {
            // Sql string for insert query
            var sqlString = "INSERT INTO \"Files\" (\"ID_LocalizationProject\", \"Name\" ,\"StringsCount\", \"Encoding\", \"IsFolder\", \"OriginalFullText\") " +
                            "VALUES (@ID_LocalizationProject, @Name , @StringsCount, @Encoding, @IsFolder, @OriginalFullText)";
            // Using new posgresql connection
            using (var connection = new NpgsqlConnection(connectionString))
            {
                IDbTransaction t = connection.BeginTransaction();
                try
                {
                    bool ans = true;
                    // Execute File insert query
                    var insertedId = await connection.ExecuteScalarAsync(sqlString, file);

                    // Return "file is uploaded" result
                    if (insertedId != null)
                    {
                        file.ID = (int)insertedId;
                        sqlString = sqlString = "INSERT INTO \"TranslationSubstrings\" (\"SubstringToTranslate\", \"Context\", \"ID_FileOwner\", \"Value\", \"PositionInText\") " +
                                                "VALUES (@SubstringToTranslate, @Context, @ID_FileOwner, @Value, @PositionInText)";
                        // Create parser object
                        using (var p = new Parser(file))
                        {
                            int n = 0;
                            n = p.TranslationSubstrings.Count;
                            foreach (var ts in p.TranslationSubstrings)
                            {
                                // Execute TranslationSubstring insert query
                                n -= await connection.ExecuteAsync(sqlString, ts);
                            }
                            ans = n == p.TranslationSubstrings.Count;
                        }
                    }
                    else ans = false;
                    t.Commit();
                    return ans;
                }
                catch (NpgsqlException exception)
                {
                    // Custom logging
                    Console.WriteLine(exception.ErrorCode);

                    return false;
                }
                catch (Exception exception)
                {
                    // Custom logging
                    Console.WriteLine(exception.Message);

                    return false;
                }
                finally
                {
                    t.Rollback();
                }
            }
        }
    }
}
