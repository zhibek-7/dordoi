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

namespace DAL.Reposity.PostgreSqlRepository
{
    public class UserRepository : BaseRepository, IRepository<User>
    {

        private PostgreSqlNativeContext context;

        public UserRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(User user)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                string SQLQuery = "INSERT INTO Users (Name, Password, Photo, Email) VALUES (@Name, @Password, @Photo, @Email)";
                dbConnection.Execute(SQLQuery, user);
                dbConnection.Close();
            }
            throw new NotImplementedException();
        }

        public User GetByID(int Id)
        {
            User user = null;
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                user = dbConnection.Query<User>("SELECT * FROM \"Users\" WHERE Id = @Id", new { Id }).FirstOrDefault();
                dbConnection.Close();
            }
            return user;
        }

        public IEnumerable<User> GetByProjectID(int Id)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<User> users = dbConnection.Query<User>(
                    "SELECT u.* FROM \"Users\" u " +
                    " join \"Participants\" p on u.\"ID\" = p.\"ID_User\" " +
                    " join \"LocalizationProjects\" lp on p.\"ID_LocalizationProject\" = lp.\"ID\" " +
                    " where lp.\"ID\" = @Id", new { Id })
                    .ToList();
                dbConnection.Close();
                return users;
            }
        }

        public bool CheckExistUser(User user)
        {
            User existUser = null;
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                existUser = dbConnection.Query<User>("SELECT * FROM \"Users\" WHERE Name = @Name AND Password = @Password", new { user.Name, user.Password }).FirstOrDefault();
                dbConnection.Close();
                if (existUser == null)
                    return true;
                return false;
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<User> users = dbConnection.Query<User>("SELECT * FROM \"Users\"").ToList();
                dbConnection.Close();
                return users;
            }
        }

        public bool Remove(int Id)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                string SQLQuery = "DELETE FROM Users WHERE Id = @Id";
                dbConnection.Execute(SQLQuery, new { Id });
                dbConnection.Close();
            }
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                string SQLQuery = "UPDATE Users SET Name = @Name, Password = @Password, Photo = @Photo, Email = @Email";
                dbConnection.Execute(SQLQuery, user);
                dbConnection.Close();
            }
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetPhotoByIdAsync(int id)
        {
            using (var dbConnection = this.context.Connection)
            {
                dbConnection.Open();
                var query = new Query("Users")
                    .Select("Photo")
                    .Where("ID", id);
                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);
                var userAvatar = await dbConnection.ExecuteScalarAsync<byte[]>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);
                dbConnection.Close();
                return userAvatar;
            }
        }

    }
}
