using Dapper;
using Models.Interfaces.Repository;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities.DTO;
using Npgsql;
using System;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class TypeOfServiceRepository : BaseRepository, ITypeOfServiceRepository
    {
        public TypeOfServiceRepository(string connectionStr) : base(connectionStr)
        {
        }


        /// <summary>
        /// Возвращает список тип услуг, содержащий только идентификатор и наименование.
        /// Для выборки, например checkbox.
        /// </summary>
        /// <returns>{id, name_text}</returns>
        public async Task<IEnumerable<TypeOfServiceForSelectDTO>> GetAllForSelectAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("type_of_service")
                        .Select("id", "name_text")
                        .OrderBy("name_text");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var result = await dbConnection.QueryAsync<TypeOfServiceForSelectDTO>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return result;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TypeOfServiceRepository)}.{nameof(TypeOfServiceRepository.GetAllForSelectAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(TypeOfServiceRepository)}.{nameof(TypeOfServiceRepository.GetAllForSelectAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }
    }
}
