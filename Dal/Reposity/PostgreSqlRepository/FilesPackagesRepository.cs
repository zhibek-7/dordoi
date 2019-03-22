﻿using System;
using System.Threading.Tasks;
using Dapper;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class FilesPackagesRepository : BaseRepository, IFilesPackagesRepository
    {

        public FilesPackagesRepository(string connectionStr) : base(connectionStr) { }

        public async Task<bool> AddAsync(FilePackage FilePackage)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("files_packages")
                        .AsInsert(FilePackage);
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var affectedRowsNumber = await connection.ExecuteAsync(compiledQuery.Sql, compiledQuery.NamedBindings);
                    return affectedRowsNumber > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesPackagesRepository)}.{nameof(FilesPackagesRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesPackagesRepository)}.{nameof(FilesPackagesRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<FilePackage> GetByFileIdAsync(int fileId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("files_packages")
                        .Where("file_id", fileId)
                        .Take(1);
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    return await connection.QueryFirstOrDefaultAsync<FilePackage>(compiledQuery.Sql, compiledQuery.NamedBindings);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesPackagesRepository)}.{nameof(FilesPackagesRepository.GetByFileIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(FilesPackagesRepository)}.{nameof(FilesPackagesRepository.GetByFileIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

    }
}