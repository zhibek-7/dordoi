using DAL.Context;
using Dapper;
using Models.DTO;
using Models.Interfaces.Repository;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class GlossariesRepository : BaseRepository, IGlossariesRepository
    {
        private readonly PostgreSqlNativeContext _context;

        public GlossariesRepository()
        {
            this._context = PostgreSqlNativeContext.getInstance();
        }

        public async Task<IEnumerable<Glossaries>> GetAllAsync()
        {
            var query = new Query("Glossaries")
                .LeftJoin("GlossariesLocales", "GlossariesLocales.ID_Glossary", "Glossaries.ID")
                .LeftJoin("Locales", "Locales.ID", "GlossariesLocales.ID_Locale")
                .LeftJoin("LocalizationProjectsGlossaries", "LocalizationProjectsGlossaries.ID_Glossary", "Glossaries.ID")
                .LeftJoin("LocalizationProjects", "LocalizationProjects.ID", "LocalizationProjectsGlossaries.ID_LocalizationProject")
                //.LeftJoin("GlossariesLocales", j => j.On("GlossariesLocales.ID_Glossary", "Glossaries.ID")
                //.LeftJoin("LocalizationProjectsGlossaries", j => j.On("LocalizationProjectsGlossaries.ID_LocalizationProject", "Glossaries.ID")
                .Select(
                    "Glossaries.ID", //"Glossaries.ID as GlossariesID",
                    "Glossaries.Name",
                    //"Glossaries.Description",
                    //"Glossaries.ID_File",
                    "Locales.ID as LocaleID",
                    "Locales.Name as LocaleName",
                    "LocalizationProjects.ID as LocalizationProjectID",
                    "LocalizationProjects.Name as LocalizationProjectName"
                );

            using (var dbConnection = this._context.Connection)
            {
                dbConnection.Open();

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);
                var glossaries = await dbConnection.QueryAsync<Glossaries>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);

                dbConnection.Close();
                return glossaries;
            }
        }

        public async Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync()
        {
            var temp = await GetAllAsync();
            var resultDTO = temp.GroupBy(t => t.ID).Select(t => new GlossariesDTO
            {
                ID = t.Key,
                Name = t.FirstOrDefault().Name,
                LocalesName = string.Join(", ", t.Select(x => x.LocaleName).Distinct().OrderBy(n => n)),
                LocalizationProjectsName = string.Join(", ", t.Select(x => x.LocalizationProjectName).Distinct().OrderBy(n => n))
            }).OrderBy(t => t.Name);
            return resultDTO;
        }



        public async Task<int> AddAsync(Glossaries item)
        {
            throw new NotImplementedException();
        }

        public async Task<Glossaries> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Glossaries item)
        {
            throw new NotImplementedException();
        }

        public Task CleanOfTermsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
