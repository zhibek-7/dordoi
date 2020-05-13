using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO.Participants;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class ParticipantRepository : BaseRepository, IParticipantRepository, IRepositoryAsync<Models.DatabaseEntities.Participant>
    {

        public ParticipantRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<Guid?> AddAsync(Participant newParticipant)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var query = new Query("participants")
                        .AsInsert(new[] {
                        "id_localization_project",
                        "id_role",
                        "id_user",
                        "active",
                        },
                        new object[]
                        {
                        newParticipant.ID_Localization_Project,
                        newParticipant.ID_Role,
                        newParticipant.ID_User,
                        newParticipant.Active
                        });

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return newParticipant.id;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public Task<IEnumerable<Participant>> GetAllAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Participant> GetByIDAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Models.DatabaseEntities.Participant updatedParticipant)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("participants")
                        .Where("id_localization_project", updatedParticipant.ID_Localization_Project)
                        .Where("id_user", updatedParticipant.ID_User)
                        .AsUpdate(new[] {
                    "id_localization_project",
                        "id_role",
                        "id_user",
                        "active",
                            },
                            new object[]
                            {
                        updatedParticipant.ID_Localization_Project,
                        updatedParticipant.ID_Role,
                        updatedParticipant.ID_User,
                        updatedParticipant.Active
                            });

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.UpdateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        private static readonly Dictionary<string, string> ParticipantsSortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id_user", "participants.id_user" },
            { "id_role", "participants.id_role" },
            { "active", "participants.active" },
            { "username", "users.name_text" },
            { "rolename", "Roles.name_text" },
        };

        public async Task<IEnumerable<ParticipantDTO>> GetByProjectIdAsync(
            Guid projectId,
            string search,
            Guid[] roleIds,
            Guid[] localeIds,
            int limit,
            int offset,
            string[] sortBy = null,
            bool sortAscending = true,
            string[] roleShort = null
            )
        {
            if (sortBy == null)
            {
                sortBy = new[] { "user_id" };
            }
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetByProjectIdQuery(
                        projectId: projectId,
                        search: search,
                        roleIds: roleIds,
                        localeIds: localeIds,
                        roleShort: roleShort);


                    query = this.ApplyPagination(
                        query: query,
                        offset: offset,
                        limit: limit);

                    query = this.ApplySorting(
                        query: query,
                        columnNamesMappings: ParticipantRepository.ParticipantsSortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var getParticipantsByProjectIdCompiledQuery = this._compiler.Compile(query);
                    this.LogQuery(getParticipantsByProjectIdCompiledQuery);
                    var participants = await dbConnection.QueryAsync<ParticipantDTO>(
                        sql: getParticipantsByProjectIdCompiledQuery.Sql,
                        param: getParticipantsByProjectIdCompiledQuery.NamedBindings);
                    return participants;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetByProjectIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetByProjectIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        protected async Task<bool> InactiveParticipantsContainsAsync(Guid projectId, Guid userId)
        {
            using (var dbConnection = new NpgsqlConnection(connectionString))
            {
                var query = new Query("participants")
                    .Where("participants.id_localization_project", projectId)
                    .Where("participants.id_user", userId)
                    .Where("participants.active", false);
                try
                {

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var participants = await dbConnection.QueryAsync<Models.DatabaseEntities.Participant>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    return participants.Any();
                }
                catch (NpgsqlException exception)
                {
                    this._loggerError.WriteLn(
                        $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.InactiveParticipantsContainsAsync)} {nameof(NpgsqlException)} ",
                        exception);
                    return false;
                }
                catch (Exception exception)
                {
                    this._loggerError.WriteLn(
                        $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.InactiveParticipantsContainsAsync)} {nameof(Exception)} ",
                        exception);
                    return false;
                }
            }
        }

        public async Task AddOrActivateParticipant(Guid projectId, Guid userId, Guid roleId)
        {
            var participant = new Models.DatabaseEntities.Participant()
            {
                ID_Localization_Project = projectId,
                ID_User = userId,
                ID_Role = roleId,
                Active = true
            };
            var inactiveContainsUser = await this.InactiveParticipantsContainsAsync(projectId: projectId, userId: userId);
            if (inactiveContainsUser)
            {
                await this.UpdateAsync(updatedParticipant: participant);
            }
            else
            {
                await this.AddAsync(newParticipant: participant);
            }
        }

        public async Task SetInactiveAsync(Guid projectId, Guid userId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("participants")
                        .Where("id_localization_project", projectId)
                        .Where("id_user", userId)
                        .AsUpdate(new[] { "active" }, new object[] { false });

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.SetInactiveAsync)} {nameof(NpgsqlException)} ",
                    exception);

            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.SetInactiveAsync)} {nameof(Exception)} ",
                    exception);

            }

        }


        public async Task<int?> GetAllByProjectIdCountAsync(
            Guid projectId,
            string search,
            Guid[] roleIds,
            Guid[] localeIds
            )
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = this.GetByProjectIdQuery(
                        projectId: projectId,
                        search: search,
                        roleIds: roleIds,
                        localeIds: localeIds)
                        .AsCount();

                    var getParticipantsCountCompiledQuery = this._compiler.Compile(query);
                    this.LogQuery(getParticipantsCountCompiledQuery);
                    var participantsCount = await dbConnection.ExecuteScalarAsync<int>(
                        sql: getParticipantsCountCompiledQuery.Sql,
                        param: getParticipantsCountCompiledQuery.NamedBindings);


                    return participantsCount;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetAllByProjectIdCountAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetAllByProjectIdCountAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="search"></param>
        /// <param name="roleIds"></param>
        /// <param name="localeIds"></param>
        /// <returns></returns>
        private Query GetByProjectIdQuery(Guid projectId, string search, Guid[] roleIds, Guid[] localeIds, string[] roleShort = null)
        {
            var query = new Query("participants")
                .Where("participants.id_localization_project", projectId)
                .Where("participants.active", true)
                .LeftJoin("users", "participants.id_user", "users.id")
                .LeftJoin("roles", "participants.id_role", "roles.id")
                .Select(
                    "participants.id_localization_project as Localization_Project_Id",
                    "participants.id_user as User_Id",
                    "participants.id_role as Role_Id",
                    "participants.active",
                    "users.name_text as User_Name",
                    "roles.name_text as Role_Name",
                    "roles.short as Role_Short"
                );

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var searchPattern = $"%{search}%";
                    query = query.WhereLike("users.name_text", searchPattern);
                }

                if (roleIds != null && roleIds.Length > 0)
                {
                    query = query.WhereIn("participants.id_role", roleIds);
                }

                if (localeIds != null && localeIds.Length > 0)
                {
                    query = query
                        .WhereExists(
                            new Query("users_locales")
                            .HavingRaw("COUNT(*)=?", localeIds.Length)
                            .WhereRaw("users_locales.id_user=participants.id_user")
                            .WhereIn("users_locales.id_locale", localeIds));
                }

                if (roleShort != null && roleShort.Length > 0)
                {
                    query = query.WhereIn("roles.short", roleShort);
                }

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);
                return query;
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetByProjectIdQuery)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetByProjectIdQuery)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

        /// <summary>
        /// Возвращает true если пользователь является владельцем хотя бы одного проекта.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool?> IsOwnerInAnyProject(string userName)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("participants")
                        .LeftJoin("roles", "roles.id", "participants.id_role")
                        .LeftJoin("users", "users.id", "participants.id_user")
                        .Where("users.name_text", userName)
                        .Where("roles.short", "owner")
                        .AsCount();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return count > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.IsOwnerInAnyProject)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.IsOwnerInAnyProject)} {nameof(Exception)} ", exception);
                return null;
            }
        }
    }
}
