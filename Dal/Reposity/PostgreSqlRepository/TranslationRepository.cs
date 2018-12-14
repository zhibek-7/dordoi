﻿using System;
using System.Collections.Generic;
using System.Text;
using DAL.Context;
using System.Data;
using Dapper;
using Models.DatabaseEntities;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    /// <summary>
    /// Репозиторий для работы с вариантами перевода фраз
    /// </summary>
    public class TranslationRepository : BaseRepository, IRepositoryAsync<Translation>
    {
        private PostgreSqlNativeContext context;

        public TranslationRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Функция добавления варианта перевода
        /// </summary>
        /// <param name="item">Вариант перевода</param>
        public async Task<int> AddAsync(Translation item)
        {
            var query = "INSERT INTO \"Translations\" (\"ID_String\", \"Translated\", \"Confirmed\", \"ID_User\", \"DateTime\", \"ID_Locale\")" +
                        "VALUES (@ID_String, @Translated, @Confirmed, @ID_User, @DateTime, @ID_Locale) " +
                        "RETURNING  \"Translations\".\"ID\"";   

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<int>(query, item);
                    dbConnection.Close();
                    return idOfInsertedRow;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return 0;
            }
        }

        /// <summary>
        /// Функция получения всех переводов
        /// </summary>
        /// <returns>Список переводов</returns>
        public async Task<IEnumerable<Translation>> GetAllAsync()
        {
            var query = @"SELECT * FROM 'Translations'";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Translation> translations = await dbConnection.QueryAsync<Translation>(query);
                    dbConnection.Close();
                    return translations;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
        }


        /// <summary>
        /// Функция получения варианта перевода по конкретному id
        /// </summary>
        /// <param name="id">id необходимого варианта перевода</param>
        /// <returns>Вариант перевода</returns>
        public async Task<Translation> GetByIDAsync(int id)
        {
            var query = "SELECT * FROM \"Translations\" WHERE \"ID\" = @id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var translation = await dbConnection.QuerySingleOrDefaultAsync<Translation>(query, new { id });
                    dbConnection.Close();

                    return translation;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }

        }

        /// <summary>
        /// Функция удаления варианта перевода по конкретному id
        /// </summary>
        /// <param name="id">id варианта перевода который нужно удалить</param>
        public async Task<bool> RemoveAsync(int id)
        {
            var query = "DELETE " +
                        "FROM \"Translations\" AS T " +
                        "WHERE T.\"ID\" = @id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var deletedRows = await dbConnection.ExecuteAsync(query, new { id });
                    dbConnection.Close();

                    return deletedRows > 0;
                }
            }            
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return false;
            }
        }

        /// <summary>
        /// Функция обновления варианта перевода
        /// </summary>
        /// <param name="item">Обновленный вариант перевода</param>
        public async Task<bool> UpdateAsync(Translation item)
        {
            try
            {
                using (var dbConnection = this.context.Connection)
                {
                    dbConnection.Open();
                    var updateTranslationSql =
                        "UPDATE \"Translations\" SET " +
                        "\"ID_String\"=@ID_String, " +
                        "\"Translated\"=@Translated, " +
                        "\"Confirmed\"=@Confirmed, " +
                        "\"ID_User\"=@ID_User, " +
                        "\"DateTime\"=@DateTime, " +
                        "\"ID_Locale\"=@ID_Locale " +
                        "WHERE \"ID\"=@ID";
                    var updateTranslationParam = item;
                    this.LogQuery(updateTranslationSql, updateTranslationParam);
                    await dbConnection.ExecuteAsync(
                        sql: updateTranslationSql,
                        param: updateTranslationParam);
                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exception)
            {
                this._logger.WriteExceprion("Exception on trying to update translation.", exception);
                return false;
            }
        }

        /// <summary>
        /// Функция получения всех вариантов перевода конкретной фразы
        /// </summary>
        /// <param name="idString">id фразы, варианты перевода которой необходимы</param>
        /// <returns>Список вариантов перевода</returns>
        public async Task<IEnumerable<Translation>> GetAllTranslationsInStringByID(int idString)
        {
            var query = "SELECT * " +
                        "FROM \"Translations\" " +
                        "WHERE \"ID_String\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Translation> translations = await dbConnection.QueryAsync<Translation>(query, new { Id = idString });
                    dbConnection.Close();
                    return translations;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

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
            var query = "UPDATE \"Translations\" " +
                        "SET \"Confirmed\" = true " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var updatedRows = await dbConnection.ExecuteAsync(query, new { Id = idTranslation });
                    dbConnection.Close();

                    return updatedRows > 0;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

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
            var query = "UPDATE \"Translations\" " +
                        "SET \"Confirmed\" = false " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var updatedRows = await dbConnection.ExecuteAsync(query, new { Id = idTranslation });
                    dbConnection.Close();

                    return updatedRows > 0;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return false;
            }
        }

    }
}
