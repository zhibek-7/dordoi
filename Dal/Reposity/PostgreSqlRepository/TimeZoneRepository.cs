using Dapper;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Npgsql;
using System;
using TimeZone = Models.DatabaseEntities.TimeZone;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class TimeZoneRepository : BaseRepository
    {
        public TimeZoneRepository(string connectionStr) : base(connectionStr)
        {
        }

        /// <summary>
        /// Возвращает все строки запроса (без группировки по объектам).
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TimeZone>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("time_zones")
                        .Select("time_zones.*");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var timeZones = await dbConnection.QueryAsync<TimeZone>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return timeZones;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TimeZoneRepository)}.{nameof(TimeZoneRepository.GetAllAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TimeZoneRepository)}.{nameof(TimeZoneRepository.GetAllAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

    }
}
