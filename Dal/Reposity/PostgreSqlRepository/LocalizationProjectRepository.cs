using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class LocalizationProjectRepository: IRepository<LocalizationProjects>
    {
        private PostgreSqlNativeContext context;

        public LocalizationProjectRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(LocalizationProjects locale)
        {
            throw new NotImplementedException();
        }

        public LocalizationProjects GetByID(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LocalizationProjects> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<LocalizationProjects> users = dbConnection.Query<LocalizationProjects>("SELECT * FROM \"Locales\"").ToList();
                dbConnection.Close();
                return users;
            }
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(LocalizationProjects user)
        {
            throw new NotImplementedException();
        }
    }
}
