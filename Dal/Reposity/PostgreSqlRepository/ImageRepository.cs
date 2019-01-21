using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Models.DatabaseEntities;
using DAL.Context;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class ImageRepository : BaseRepository, IRepository<Image>
    {
        public ImageRepository(string connectionStr) : base(connectionStr)
        {
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
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                IEnumerable<Image> images = dbConnection.Query<Image>("SELECT * FROM \"Images\"").ToList();
                return images;
            }
        }

        public bool Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Image user)
        {
            throw new NotImplementedException();
        }
    }
}
