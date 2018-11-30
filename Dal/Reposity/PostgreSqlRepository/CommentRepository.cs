using System;
using System.Collections.Generic;
using System.Text;

using Dapper;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using System.Linq;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class CommentRepository : IRepository<Comments>
    {
        private PostgreSqlNativeContext context;

        public CommentRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(Comments item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comments> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Comments> comments = dbConnection.Query<Comments>("SELECT * FROM \"Comments\"").ToList();
                dbConnection.Close();
                return comments;
            }
        }

        public Comments GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Comments item)
        {
            throw new NotImplementedException();
        }
    }
}
