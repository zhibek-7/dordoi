using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO.Participants;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class ParticipantRepository : BaseRepository, IRepositoryAsync<Models.DatabaseEntities.Participant>
    {

        private readonly PostgreSqlNativeContext _context = PostgreSqlNativeContext.getInstance();

        public async Task<int> AddAsync(Participant newParticipant)
        {
            try {

                using (var dbConnection = this._context.Connection)
                {
                    dbConnection.Open();

                    var query = new Query("Participants")
                        .AsInsert(new[] {
                        "ID_LocalizationProject",
                        "ID_Role",
                        "ID_User",
                        "Active",
                        },
                        new object[]
                        {
                        newParticipant.ID_LocalizationProject,
                        newParticipant.ID_Role,
                        newParticipant.ID_User,
                        newParticipant.Active
                        });

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    dbConnection.Close();

                    return newParticipant.ID;
                }

            }


            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }


        }

        public Task<IEnumerable<Models.DatabaseEntities.Participant>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Models.DatabaseEntities.Participant> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Models.DatabaseEntities.Participant updatedParticipant)
        {
            try {
                using (var dbConnection = this._context.Connection)
                {
                    dbConnection.Open();
                    var query = new Query("Participants")
                        .Where("ID_LocalizationProject", updatedParticipant.ID_LocalizationProject)
                        .Where("ID_User", updatedParticipant.ID_User)
                        .AsUpdate(new[] {
                        "ID_LocalizationProject",
                        "ID_Role",
                        "ID_User",
                        "Active",
                        },
                        new object[]
                        {
                        updatedParticipant.ID_LocalizationProject,
                        updatedParticipant.ID_Role,
                        updatedParticipant.ID_User,
                        updatedParticipant.Active
                        });

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    dbConnection.Close();
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
            { "id_user", "Participants.ID_User" },
            { "id_role", "Participants.ID_Role" },
            { "active", "Participants.Active" },
            { "username", "Users.Name" },
            { "rolename", "Roles.Name" },
        };

        public async Task<IEnumerable<ParticipantDTO>> GetByProjectIdAsync(
            int projectId,
            string search,
            int[] roleIds,
            int[] localeIds,
            int limit,
            int offset,
            string[] sortBy = null,
            bool sortAscending = true
            )
        {
            if (sortBy == null)
            {
                sortBy = new[] { "user_id" };
            }

            try {
                    using (var dbConnection = this._context.Connection)
                        {
                            dbConnection.Open();
                            var query = this.GetByProjectIdQuery(
                                projectId: projectId,
                                search: search,
                                roleIds: roleIds,
                                localeIds: localeIds);

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
                            dbConnection.Close();
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

        protected async Task<bool> InactiveParticipantsContainsAsync(int projectId, int userId)
        {
            try {
                using (var dbConnection = this._context.Connection)
                    {
                        dbConnection.Open();
                        var query = new Query("Participants")
                            .Where("Participants.ID_LocalizationProject", projectId)
                            .Where("Participants.ID_User", userId)
                            .Where("Participants.Active", false);

                        var compiledQuery = this._compiler.Compile(query);
                        this.LogQuery(compiledQuery);
                        var participants = await dbConnection.QueryAsync<Models.DatabaseEntities.Participant>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
                        dbConnection.Close();
                        return participants.Any();
                    }
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

        public async Task AddOrActivateParticipant(int projectId, int userId, int roleId)
        {
            var participant = new Models.DatabaseEntities.Participant()
            {
                ID_LocalizationProject = projectId,
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

        public async Task SetInactiveAsync(int projectId, int userId)
        {
            try {
                using (var dbConnection = this._context.Connection)
                {
                    dbConnection.Open();
                    var query = new Query("Participants")
                        .Where("ID_LocalizationProject", projectId)
                        .Where("ID_User", userId)
                        .AsUpdate(new[] { "Active" }, new object[] { false });

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);
                    dbConnection.Close();
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

        public async Task<int> GetAllByProjectIdCountAsync(
            int projectId,
            string search,
            int[] roleIds,
            int[] localeIds
            )
        {
            try {

                using (var dbConnection = this._context.Connection)
                {
                    dbConnection.Open();

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

                    dbConnection.Close();
                    return participantsCount;
                }
            }

          catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetAllByProjectIdCountAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ParticipantRepository)}.{nameof(ParticipantRepository.GetAllByProjectIdCountAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
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
        private Query GetByProjectIdQuery(int projectId, string search, int[] roleIds, int[] localeIds)
        {
            var query = new Query("Participants")
                .Where("Participants.ID_LocalizationProject", projectId)
                .Where("Participants.Active", true)
                .LeftJoin("Users", "Participants.ID_User", "Users.ID")
                .LeftJoin("Roles", "Participants.ID_Role", "Roles.ID")
                .Select(
                    "Participants.ID_LocalizationProject as LocalizationProjectId",
                    "Participants.ID_User as UserId",
                    "Participants.ID_Role as RoleId",
                    "Participants.Active",
                    "Users.Name as UserName",
                    "Roles.Name as RoleName"
                );

            try {
                if (!string.IsNullOrEmpty(search))
                {
                    var searchPattern = $"%{search}%";
                    query = query.WhereLike("Users.Name", searchPattern);
                }

                if (roleIds != null && roleIds.Length > 0)
                {
                    query = query.WhereIn("Participants.ID_Role", roleIds);
                }

                if (localeIds != null && localeIds.Length > 0)
                {
                    query = query
                        .WhereExists(
                            new Query("UsersLocales")
                            .HavingRaw("COUNT(*)=?", localeIds.Length)
                            .WhereRaw("\"UsersLocales\".\"ID_User\"=\"Participants\".\"ID_User\"")
                            .WhereIn("UsersLocales.ID_Locale", localeIds));
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

    }
}
