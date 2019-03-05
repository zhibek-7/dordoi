using System;
using System.Threading.Tasks;
using Dapper;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class InvitationsRepository : BaseRepository, IInvitationsRepository
    {

        public InvitationsRepository(string connectionStr)
            : base(connectionStr) { }

        public async Task<bool> AddAsync(Invitation invitation)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("invitations")
                        .AsInsert(invitation);
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var numberOfRowsAffected = await connection.ExecuteAsync(compiledQuery.Sql, compiledQuery.NamedBindings);
                    return numberOfRowsAffected > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(InvitationsRepository)}.{nameof(InvitationsRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(InvitationsRepository)}.{nameof(InvitationsRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<Invitation> GetByIdAsync(Guid id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("invitations")
                        .Where("id", id);
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    return await connection.QueryFirstAsync<Invitation>(compiledQuery.Sql, compiledQuery.NamedBindings);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(InvitationsRepository)}.{nameof(InvitationsRepository.GetByIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(InvitationsRepository)}.{nameof(InvitationsRepository.GetByIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

    }
}
