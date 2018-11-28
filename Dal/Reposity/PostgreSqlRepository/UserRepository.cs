using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using Dapper;
using System.Data;
using System.Linq;
using DAL.Context;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class UserRepository: IRepository<User>
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

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
