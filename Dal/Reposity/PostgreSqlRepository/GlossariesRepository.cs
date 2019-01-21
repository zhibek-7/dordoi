using DAL.Context;
using Dapper;
using Models.Interfaces.Repository;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;

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

        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllToDTOAsync()
        {
            var temp = await GetAllAsync();
            var resultDTO = temp.GroupBy(t => t.ID).Select(t => new GlossariesTableViewDTO
            {
                ID = t.Key,
                Name = t.FirstOrDefault().Name,
                LocalesName = string.Join(", ", t.Select(x => x.LocaleName).Distinct().OrderBy(n => n)),
                LocalizationProjectsName = string.Join(", ", t.Select(x => x.LocalizationProjectName).Distinct().OrderBy(n => n))
            }).OrderBy(t => t.Name);
            return resultDTO;
        }

        public async Task AddNewGlossaryAsync(GlossariesForEditing glossary)
        {
            try
            {
                var newGlossaries = new
                {
                    Name = glossary.Name,
                    Description = glossary.Description,
                    ID_File = (int?)null
                };
                var query = new Query("Glossaries").AsInsert(newGlossaries, true);

                using (var dbConnection = this._context.Connection)
                {
                    dbConnection.Open();

                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);

                    var idOfNewGlossary = await dbConnection
                        .ExecuteScalarAsync<int>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings
                            );

                    var GlossariesLocales = glossary.Locales.Select(t => new
                    {
                        ID_Glossary = idOfNewGlossary,
                        ID_Locale = t.ID
                    }).ToList();
                    foreach (var element in GlossariesLocales)
                    {
                        var queryGlossariesLocales = new Query("GlossariesLocales").AsInsert(element);
                        var compiledQueryGlossariesLocales = this._compiler.Compile(queryGlossariesLocales);
                        this.LogQuery(compiledQueryGlossariesLocales);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryGlossariesLocales.Sql,
                                param: compiledQueryGlossariesLocales.NamedBindings
                                );
                    }


                    var LocalizationProjectsGlossaries = glossary.LocalizationProjects.Select(t => new
                    {
                        ID_Glossary = idOfNewGlossary,
                        ID_LocalizationProject = t.ID
                    }).ToList();
                    foreach (var element in LocalizationProjectsGlossaries)
                    {
                        var queryLocalizationProjectsGlossaries = new Query("LocalizationProjectsGlossaries").AsInsert(element);
                        var compiledQueryLocalizationProjectsGlossaries = this._compiler.Compile(queryLocalizationProjectsGlossaries);
                        this.LogQuery(compiledQueryLocalizationProjectsGlossaries);
                        await dbConnection.ExecuteAsync(
                                sql: compiledQueryLocalizationProjectsGlossaries.Sql,
                                param: compiledQueryLocalizationProjectsGlossaries.NamedBindings
                                );
                    }
                    dbConnection.Close();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteGlossaryAsync(int id)
        {
            try
            {
                using (var dbConnection = this._context.Connection)
                {
                    dbConnection.Open();
                    /*
                    при удалени глосария нужно удалить файл, строки, термины и переводы
                      
                    все из TranslationSubstrings и Files удалю по Glossaries.ID_File                    
                    еще Translations (ID_String), на них CommentsImages(ID_Comment),Comments(ID_TranslationSubstrings)

                    констрейны нужно сделать, что бы автоматом
                    */


                    //var queryGlossariesStrings = new Query("GlossariesStrings").Where("ID_Glossary", id).AsDelete();
                    //var compiledQueryGlossariesStrings = this._compiler.Compile(queryGlossariesStrings);
                    //this.LogQuery(compiledQueryGlossariesStrings);
                    //await dbConnection.ExecuteAsync(
                    //    sql: compiledQueryGlossariesStrings.Sql,
                    //    param: compiledQueryGlossariesStrings.NamedBindings
                    //);

                    //var queryTranslationSubstrings = new Query("TranslationSubstrings").Where("ID_FileOwner", fileId).AsDelete();
                    //var compiledQueryTranslationSubstrings = this._compiler.Compile(queryTranslationSubstrings);
                    //this.LogQuery(compiledQueryTranslationSubstrings);
                    //await dbConnection.ExecuteAsync(
                    //    sql: compiledQueryTranslationSubstrings.Sql,
                    //    param: compiledQueryTranslationSubstrings.NamedBindings
                    //);





                    var queryGlossariesLocales = new Query("GlossariesLocales").Where("ID_Glossary", id).AsDelete();
                    var compiledQueryGlossariesLocales = this._compiler.Compile(queryGlossariesLocales);
                    this.LogQuery(compiledQueryGlossariesLocales);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryGlossariesLocales.Sql,
                        param: compiledQueryGlossariesLocales.NamedBindings
                    );

                    var queryLocalizationProjectsGlossaries = new Query("LocalizationProjectsGlossaries").Where("ID_Glossary", id).AsDelete();
                    var compiledQueryLocalizationProjectsGlossaries = this._compiler.Compile(queryLocalizationProjectsGlossaries);
                    this.LogQuery(compiledQueryLocalizationProjectsGlossaries);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryLocalizationProjectsGlossaries.Sql,
                        param: compiledQueryLocalizationProjectsGlossaries.NamedBindings
                    );

                    var query = new Query("Glossaries").Where("ID", id).AsDelete();
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );

                    dbConnection.Close();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
