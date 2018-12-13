using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectRepository: IRepository<LocalizationProject>
    {
        private PostgreSqlNativeContext context;

        public LocalizationProjectRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(LocalizationProject locale)
        {
            throw new NotImplementedException();
        }

        public LocalizationProject GetByID(int Id)
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM \"LocalizationProjects\" WHERE \"ID\" = @Id";

            try
            {
                // Using new posgresql connection
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var project = dbConnection.Query<LocalizationProject>(sqlString, new {Id}).FirstOrDefault();
                    dbConnection.Close();
                    return project;
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

                return null;
            }
        }

        public IEnumerable<LocalizationProject> GetAll()
        {
            // Sql string to select all rows
            var sqlString = "SELECT * FROM \"LocalizationProjects\"";

            try
            {
                // Using new posgresql connection
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<LocalizationProject> users = dbConnection.Query<LocalizationProject>(sqlString);
                    dbConnection.Close();
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                // Custom logging
                Console.WriteLine(exception.ErrorCode);

                return null;
            }
            catch (Exception exception)
            {
                // Custom logging
                Console.WriteLine(exception.Message);

                return null;
            }
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(LocalizationProject user)
        {
            throw new NotImplementedException();
        }
    }
}
