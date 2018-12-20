using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using Dapper;
using Models.Participants;
using SqlKata;
using SqlKata.Compilers;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class ParticipantRepository : BaseRepository, IRepositoryAsync<Participant>
    {

        private readonly PostgreSqlNativeContext _context = PostgreSqlNativeContext.getInstance();

        private readonly Compiler _compiler = new PostgresCompiler();

        public Task<int> AddAsync(Participant item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Participant>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Participant> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Participant item)
        {
            throw new NotImplementedException();
        }

        private static readonly Dictionary<string, string> ParticipantsSortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id_user", "Participants.ID_User" },
            { "id_role", "Participants.ID_Role" },
            { "active", "Participants.Active" },
            { "username", "Users.Name" },
            { "rolename", "Roles.Name" },
        };

        public async Task<IEnumerable<Participant>> GetByProjectIdAsync(
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
                var participants = await dbConnection.QueryAsync<Participant>(
                    sql: getParticipantsByProjectIdCompiledQuery.Sql,
                    param: getParticipantsByProjectIdCompiledQuery.NamedBindings);
                dbConnection.Close();
                return participants;
            }
        }

        public async Task<int> GetAllByProjectIdCountAsync(
            int projectId,
            string search,
            int[] roleIds,
            int[] localeIds
            )
        {
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

            return query;
        }

    }
}
