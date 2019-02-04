using System;
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
                    string SQLQuery = "SELECT * FROM \"Users\" WHERE (\"Name\" = @Name OR \"Email\" = @Email) AND \"Password\" = @Password";
                    User existUser = null;
                    var param = new { user.Name, user.Email, user.Password };
                    this.LogQuery(SQLQuery, param);
                    existUser = dbConnection.Query<User>(SQLQuery, param).FirstOrDefault();                    
                    return existUser;
                
                    //var password = Utilities.Cryptography.CryptographyProvider.GetMD5Hash(user.Password);
                    //var query = new Query("Users")
                    //    .Where("Password", password)
                    //    .Where("Name", user.Name)
                    //    .Or()
                    //    .Where("Password", password)
                    //    .Where("Email", user.Name)
                    //    .Select("*");
                    //var compiledQuery = _compiler.Compile(query);
                    //LogQuery(compiledQuery);
                    
                    //var result = await dbConnection
                    //    .QueryFirstOrDefaultAsync<User>(
                    //        sql: compiledQuery.Sql,
                    //        param: compiledQuery.NamedBindings);
                    //return result;
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
    }
}
