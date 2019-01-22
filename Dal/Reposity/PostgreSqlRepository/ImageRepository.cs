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
    /// <summary>
    /// ///нужно ли здесь ставить  this.LogQuery(sqlString); не поняла, так как в filerepository в примерно таких запросах  this.LogQuery(sqlString); нет
    /// </summary>
    public class ImageRepository : BaseRepository, IRepository<Image>
    {
        private PostgreSqlNativeContext context;
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
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sqlString = "SELECT * FROM \"Images\"";
                    this.LogQuery(sqlString);
                    IEnumerable<Image> images = dbConnection.Query<Image>(sqlString).ToList();
                    return images;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.GetAll)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.GetAll)} {nameof(Exception)} ",
                    exception);
                return null;
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
