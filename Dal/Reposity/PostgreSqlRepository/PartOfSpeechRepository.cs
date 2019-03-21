using DAL.Context;
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

    }
}
