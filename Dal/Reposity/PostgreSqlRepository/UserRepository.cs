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
            try {

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
            try {
                User user = null;
                string SQLQuery = "SELECT * FROM \"Users\" WHERE Id = @Id";
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id };
                    this.LogQuery(SQLQuery, param);
                    user = dbConnection.Query<User>("SELECT * FROM \"Users\" WHERE Id = @Id", param).FirstOrDefault();
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
            try {
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
            try {
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
            try {
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
            try {
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
            try {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(SQLQuery, param:user);
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
            try {

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

    }
}
