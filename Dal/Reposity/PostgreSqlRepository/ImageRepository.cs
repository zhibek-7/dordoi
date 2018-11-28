using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Models.DatabaseEntities;
using DAL.Context;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class ImageRepository: IRepository<Image>
    {
        private PostgreSqlNativeContext context;

        public ImageRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public void Add(Image user)
        {
            throw new NotImplementedException();
        }

        public Image GetByID(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Image> GetAll()
        {
            using (IDbConnection dbConnection = context.Connection)
            {
                dbConnection.Open();
                IEnumerable<Image> images = dbConnection.Query<Image>("SELECT * FROM \"Images\"").ToList();
                dbConnection.Close();
                return images;
            }
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Image user)
        {
            throw new NotImplementedException();
        }
    }
}
