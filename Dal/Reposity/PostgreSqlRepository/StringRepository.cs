using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class StringRepository : IRepositoryAsync<Models.DatabaseEntities.String>
    {
        private PostgreSqlNativeContext context;

        public StringRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public Task<int> Add(Models.DatabaseEntities.String item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Models.DatabaseEntities.String>> GetAll()
        {
            var query = "SELECT * FROM \"TranslationSubstrings\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Models.DatabaseEntities.String> strings = await dbConnection.QueryAsync<Models.DatabaseEntities.String>(query);
                    dbConnection.Close();
                    return strings;
                }
            }
            catch(Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
            
        }

        public async Task<Models.DatabaseEntities.String> GetByID(int id)
        {
            var query = "SELECT * " +
                        "FROM \"Strings\" " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var foundedString = await dbConnection.QuerySingleAsync<Models.DatabaseEntities.String>(query, new { Id = id });
                    dbConnection.Close();
                    return foundedString;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
            
        }

        public Task<bool> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Models.DatabaseEntities.String item)
        {
            throw new NotImplementedException();
        }
    }
}
