using Dapper;
using Models.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Models.Interfaces.Repository;
using Npgsql;
using SqlKata;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class PartOfSpeechRepository : BaseRepository, IRepository<PartOfSpeech>
    {

        public PartOfSpeechRepository(string connectionStr) : base(connectionStr)
        {
        }

        //public void Add(PartOfSpeech item)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<PartOfSpeech> GetAll()
        {
            throw new NotImplementedException();
        }

        //public PartOfSpeech GetByID(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Remove(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(PartOfSpeech item)
        //{
        //    throw new NotImplementedException();
        //}
        public IEnumerable<PartOfSpeech> GetPartOfSpeechByGlossaryId(Guid glossaryId)
        {
            var query = "select gs.ID_Glossary, " +
                "gs.ID_PartOfSpeech," +
                "gs.ID_String, " +
                "ps.Name," +
                "ps.LocaleID," +
                " from GlossariesStrings as gs" +
                "inner join Glossaries as g on g.ID = gs.ID_Glossary" +
                "inner join PartsOfSpeech as ps on ps.ID = gs.ID_PartOfSpeech" +
                "where g.ID = @glossaryId";
            try
            {

                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { Id = glossaryId };
                    this.LogQuery(query, param);
                    IEnumerable<PartOfSpeech> strings = dbConnection.Query<PartOfSpeech>(query, param);
                    return strings;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(PartOfSpeechRepository)}.{nameof(PartOfSpeechRepository.GetPartOfSpeechByGlossaryId)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(PartOfSpeechRepository)}.{nameof(PartOfSpeechRepository.GetPartOfSpeechByGlossaryId)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }
        public IEnumerable<PartOfSpeech> GetByGlossaryId(Guid glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects_glossaries as lpg")
                        .Where("lpg.id_glossary", glossaryId)
                        .RightJoin("localization_projects as lp", "lpg.id_localization_project", "lp.id")
                        .RightJoin("parts_of_speech as pos", "lp.id_source_locale", "pos.locale_id")
                        .Select("pos.*");
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var partsOfSpeechForLocale = dbConnection
                        .Query<PartOfSpeech>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
                    return partsOfSpeechForLocale;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(PartOfSpeechRepository)}.{nameof(PartOfSpeechRepository.GetByGlossaryId)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(PartOfSpeechRepository)}.{nameof(PartOfSpeechRepository.GetByGlossaryId)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }


        /// <summary>
        /// Получение записи по id термина
        /// </summary>
        /// <param name="termId"></param>
        /// <returns></returns>
        public IEnumerable<PartOfSpeech> GetByTermId(Guid? termId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("localization_projects_glossaries as lpg")
                        .Join("glossaries_strings as gs", "lpg.id_glossary", "gs.id_glossary")
                        .Where("gs.id_string", termId)
                        .RightJoin("localization_projects as lp", "lpg.id_localization_project", "lp.id")
                        .RightJoin("parts_of_speech as pos", "lp.id_source_locale", "pos.locale_id")
                        .Select("pos.*");
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var partsOfSpeechForLocale = dbConnection
                        .Query<PartOfSpeech>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
                    return partsOfSpeechForLocale;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(PartOfSpeechRepository)}.{nameof(PartOfSpeechRepository.GetByTermId)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(PartOfSpeechRepository)}.{nameof(PartOfSpeechRepository.GetByTermId)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

    }
}
