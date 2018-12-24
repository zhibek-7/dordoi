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

namespace DAL.Reposity.PostgreSqlRepository
{
    public class UserRepository: BaseRepository, IRepository<User>
    {

        private PostgreSqlNativeContext context;
        
        public UserRepository()
        {
            context = PostgreSqlNativeContext.getInstance();          
        }

        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public User GetByID(int Id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
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
