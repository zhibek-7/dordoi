using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Models.DatabaseEntities;
using DAL.Context;
using Models.Interfaces.Repository;
using Npgsql;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    /// <summary>
    ///
    /// </summary>
    public class ImageRepository : BaseRepository, IBaseRepositoryAsync<Image>
    {
        private PostgreSqlNativeContext context;
        public ImageRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<IEnumerable<Image>> GetAllAsync()
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
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

    }
}
