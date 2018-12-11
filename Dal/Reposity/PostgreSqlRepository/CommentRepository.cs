using System;
using System.Collections.Generic;
using System.Text;

using Dapper;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class CommentRepository : IRepositoryAsync<Comments>
    {
        private PostgreSqlNativeContext context;

        public CommentRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public Task<int> Add(Comments item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Comments>> GetAll()
        {
            var query = "SELECT * FROM \"Comments\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Comments> comments = await dbConnection.QueryAsync<Comments>(query);
                    dbConnection.Close();
                    return comments;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }

        }

        public Task<Comments> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Comments item)
        {
            throw new NotImplementedException();
        }
    }
}
