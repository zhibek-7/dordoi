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
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                string SQLQuery = "INSERT INTO Users (Name, Password, Photo, Email) VALUES (@Name, @Password, @Photo, @Email)";
                dbConnection.Execute(SQLQuery, user);
            }
            throw new NotImplementedException();
        }

        public User GetByID(int Id)
        {
            User user = null;
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                user = dbConnection.Query<User>("SELECT * FROM \"Users\" WHERE Id = @Id", new { Id }).FirstOrDefault();
            }
            return user;
        }

        public IEnumerable<User> GetByProjectID(int Id)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                IEnumerable<User> users = dbConnection.Query<User>(
                    "SELECT u.* FROM \"Users\" u " +
                    " join \"Participants\" p on u.\"ID\" = p.\"ID_User\" " +
                    " join \"LocalizationProjects\" lp on p.\"ID_LocalizationProject\" = lp.\"ID\" " +
                    " where lp.\"ID\" = @Id", new { Id })
                    .ToList();
                return users;
            }
        }

        public bool CheckExistUser(User user)
        {
            User existUser = null;
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                existUser = dbConnection.Query<User>("SELECT * FROM \"Users\" WHERE Name = @Name AND Password = @Password", new { user.Name, user.Password }).FirstOrDefault();
                if (existUser == null)
                    return true;
                return false;
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                IEnumerable<User> users = dbConnection.Query<User>("SELECT * FROM \"Users\"").ToList();
                return users;
            }
        }

        public bool Remove(int Id)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                string SQLQuery = "DELETE FROM Users WHERE Id = @Id";
                dbConnection.Execute(SQLQuery, new { Id });
            }
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                string SQLQuery = "UPDATE Users SET Name = @Name, Password = @Password, Photo = @Photo, Email = @Email";
                dbConnection.Execute(SQLQuery, user);
            }
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetPhotoByIdAsync(int id)
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

    }
}
