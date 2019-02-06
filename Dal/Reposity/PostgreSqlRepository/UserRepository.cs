﻿using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using Dapper;
using System.Data;
using System.Linq;
using DAL.Context;
using System.Threading.Tasks;
using SqlKata;
using Models.Interfaces.Repository;
using Npgsql;
using Models.DatabaseEntities.DTO;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class UserRepository : BaseRepository, IRepository<User>
    {

        public UserRepository(string connectionStr) : base(connectionStr)
        {
        }

        public void Add(User user)
        {
            try
            {

                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    string SQLQuery = "INSERT INTO Users (Name, Password, Photo, Email) VALUES (@Name, @Password, @Photo, @Email)";
                    this.LogQuery(SQLQuery, param: user);
                    dbConnection.Execute(SQLQuery, user);
                }

            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Add)} {nameof(NpgsqlException)} ",
                    exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Add)} {nameof(Exception)} ",
                    exception);
            }
        }

        public User GetByID(int Id)
        {
            try
            {
                User user = null;
                string SQLQuery = "SELECT * FROM \"Users\" WHERE Id = @Id";
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id };
                    this.LogQuery(SQLQuery, param);
                    user = dbConnection.Query<User>(SQLQuery, param).FirstOrDefault();
                }
                return user;
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByID)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByID)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public IEnumerable<User> GetByProjectID(int Id)
        {
            try
            {
                string SQLQuery = "SELECT u.* FROM \"Users\" u " +
                        " join \"Participants\" p on u.\"ID\" = p.\"ID_User\" " +
                        " join \"LocalizationProjects\" lp on p.\"ID_LocalizationProject\" = lp.\"ID\" " +
                        " where lp.\"ID\" = @Id";
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var param = new { Id };
                    this.LogQuery(SQLQuery, param);
                    IEnumerable<User> users = dbConnection.Query<User>(SQLQuery, param)
                        .ToList();
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByProjectID)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByProjectID)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public bool CheckExistUser(User user)
        {
            string SQLQuery = "SELECT * FROM \"Users\" WHERE Name = @Name AND Password = @Password";
            try
            {
                User existUser = null;
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { user.Name, user.Password };
                    this.LogQuery(SQLQuery, param);
                    existUser = dbConnection.Query<User>(SQLQuery, param).FirstOrDefault();
                    if (existUser == null)
                        return true;
                    return false;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CheckExistUser)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CheckExistUser)} {nameof(Exception)} ",
                    exception);
                return false;
            }


        }

        public IEnumerable<User> GetAll()
        {
            string SQLQuery = "SELECT * FROM \"Users\"";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(SQLQuery);
                    IEnumerable<User> users = dbConnection.Query<User>(SQLQuery).ToList();
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAll)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAll)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public bool Remove(int Id)
        {
            string SQLQuery = "DELETE FROM Users WHERE Id = @Id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var param = new { Id };
                    this.LogQuery(SQLQuery, param);
                    dbConnection.Execute(SQLQuery, param);
                }
                throw new NotImplementedException();
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Remove)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Remove)} {nameof(Exception)} ",
                    exception);
                return false;
            }


        }

        public void Update(User user)
        {
            string SQLQuery = "UPDATE Users SET Name = @Name, Password = @Password, Photo = @Photo, Email = @Email";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(SQLQuery, param: user);
                    dbConnection.Execute(SQLQuery, user);
                }
                throw new NotImplementedException();
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Update)} {nameof(NpgsqlException)} ",
                    exception);
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Update)} {nameof(Exception)} ",
                    exception);
            }


        }

        public async Task<byte[]> GetPhotoByIdAsync(int id)
        {
            try
            {

                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Users")
                        .Select("Photo")
                        .Where("ID", id);
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var userAvatar = await dbConnection.ExecuteScalarAsync<byte[]>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return userAvatar;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetPhotoByIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetPhotoByIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        //
        public async Task<bool?> IsUniqueEmail(string email)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Users")
                        .Where("Email", email)
                        .AsCount();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return count == 0;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueEmail)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueEmail)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<bool?> IsUniqueLogin(string login)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Users")
                        .Where("Name", login)
                        .AsCount();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return count == 0;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueLogin)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueLogin)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<int?> CreateUser(User user)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var newUser = new
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Password = Utilities.Cryptography.CryptographyProvider.GetMD5Hash(user.Password),
                        data_create = DateTime.Now
                    };
                    var query = new Query("Users").AsInsert(newUser, true); //true - вернуть сгенерированный id нового объекта
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    //await dbConnection.ExecuteAsync(
                    //    sql: compiledQuery.Sql,
                    //    param: compiledQuery.NamedBindings);
                    //После выполнение запроса получаем сгенерированный id нового объекта
                    var idOfNewUser = await dbConnection
                        .ExecuteScalarAsync<int>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
                    return idOfNewUser;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CreateUser)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CreateUser)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<User> Login(User user)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    user.Password = Utilities.Cryptography.CryptographyProvider.GetMD5Hash(user.Password);
                    string SQLQuery = "SELECT \"ID\", \"Name\" FROM \"Users\" WHERE (\"Name\" = @Name OR \"Email\" = @Email) AND \"Password\" = @Password";
                    User existUser = null;
                    var param = new { user.Name, user.Email, user.Password };
                    LogQuery(SQLQuery, param);
                    existUser = dbConnection.Query<User>(SQLQuery, param).FirstOrDefault();                    
                    return existUser;                
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Login)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Login)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<UserProfileForEditingDTO> GetProfileAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Users")
                        .Where("Users.ID", id)
                        .LeftJoin("UsersLocales", "UsersLocales.ID_User", "Users.ID")
                        .Select(
                        "Users.*",
                        "UsersLocales.ID_Locale As LocaleId",
                        "UsersLocales.IsNative As LocaleIsNative"
                        );
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var temp = await dbConnection.QueryAsync<UserProfile>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    //Создание пользователя с вложенными списками идентификаторов связанных данных.
                    var resultDTO = new UserProfileForEditingDTO
                    {
                        ID = temp.FirstOrDefault().ID,
                        Name = temp.FirstOrDefault().Name,
                        Email = temp.FirstOrDefault().Email,
                        Photo = temp.FirstOrDefault().Photo,
                        FullName = temp.FirstOrDefault().FullName,
                        AboutMe = temp.FirstOrDefault().AboutMe,
                        Gender = temp.FirstOrDefault().Gender,
                        TimeZone = temp.FirstOrDefault().TimeZone,
                        
                        LocalesIds = temp.Select(t => t.LocaleId).Distinct(),
                        LocalesIdIsNative = temp.Select(t => Tuple.Create<int, bool>(t.LocaleId.Value, t.LocaleIsNative)).Distinct()
                    };

                    return resultDTO;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetProfileAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetProfileAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task UpdateAsync(UserProfileForEditingDTO user)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var edited = new
                    {
                        Photo = user.Photo,
                        Email = user.Email,
                        //Joined = user.Joined,
                        FullName = user.FullName,
                        TimeZone = user.TimeZone,
                        AboutMe = user.AboutMe,
                        Gender = user.Gender
                    };
                    var query = new Query("Users").Where("ID", user.ID).AsUpdate(edited);
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);


                    //Пересоздание связей пользователя с языками перевода (Users с Locales)
                    //await UpdateUsersLocalesAsync(user.ID, user.LocalesIds, user.LocalesIdIsNative);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateAsync)} {nameof(Exception)} ", exception);
            }
        }
        /// <summary>
        /// Пересоздание связей пользователя с языками перевода (Users с Locales).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="localesIds">Выбранные языки перевода.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task UpdateUsersLocalesAsync(int userId, IEnumerable<int?> localesIds, IEnumerable<Tuple<int, bool>> localesIdIsNative, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryDelete = new Query("UsersLocales").Where("ID_User", userId).AsDelete();
                        var compiledQueryDelete = _compiler.Compile(queryDelete);
                        LogQuery(compiledQueryDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryDelete.Sql,
                            param: compiledQueryDelete.NamedBindings);
                    }



                    var usersLocalesIsNative = localesIdIsNative.Select(t => new
                    {
                        ID_User = userId,
                        ID_Locale = t.Item1,
                        IsNative = t.Item2
                    }).ToList();

                    if (usersLocalesIsNative != null && usersLocalesIsNative.Count > 0)
                    {
                        foreach (var element in usersLocalesIsNative)
                        {
                            var queryInsert = new Query("UsersLocales").AsInsert(element);
                            var compiledQueryInsert = _compiler.Compile(queryInsert);
                            LogQuery(compiledQueryInsert);
                            await dbConnection.ExecuteAsync(
                                    sql: compiledQueryInsert.Sql,
                                    param: compiledQueryInsert.NamedBindings);
                        }
                    }
                    else
                    {
                        var usersLocales = localesIds.Select(t => new
                        {
                            ID_User = userId,
                            ID_Locale = t
                        }).ToList();

                        foreach (var element in usersLocales)
                        {
                            var queryInsert = new Query("UsersLocales").AsInsert(element);
                            var compiledQueryInsert = _compiler.Compile(queryInsert);
                            LogQuery(compiledQueryInsert);
                            await dbConnection.ExecuteAsync(
                                    sql: compiledQueryInsert.Sql,
                                    param: compiledQueryInsert.NamedBindings);
                        }
                    }
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateUsersLocalesAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateUsersLocalesAsync)} {nameof(Exception)} ", exception);
            }
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("Users").Where("ID", id).AsDelete();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.RemoveAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.RemoveAsync)} {nameof(Exception)} ", exception);
            }
        }
    }
}
