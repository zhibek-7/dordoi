using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using SqlKata;
using SqlKata.Compilers;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class RoleRepository : BaseRepository, IRepositoryAsync<Role>
    {

        private readonly PostgreSqlNativeContext _context = PostgreSqlNativeContext.getInstance();

        private readonly Compiler _compiler = new PostgresCompiler();

        public Task<int> AddAsync(Role item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            using (var dbConnection = _context.Connection)
            {
                dbConnection.Open();
                var query = new Query("Roles");
                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);
                var roles = await dbConnection.QueryAsync<Role>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);
                dbConnection.Close();
                return roles;
            }
        }

        public Task<Role> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Role item)
        {
            throw new NotImplementedException();
        }

    }
}
