using DAL.Context;
using Dapper;
using Models.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class PartOfSpeechRepository : BaseRepository, IRepository<PartOfSpeech>
    {

        public PartOfSpeechRepository(string connectionStr) : base(connectionStr)
        {
        }

        public void Add(PartOfSpeech item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PartOfSpeech> GetAll()
        {
            throw new NotImplementedException();
        }

        public PartOfSpeech GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PartOfSpeech item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PartOfSpeech> GetByGlossaryId(int glossaryId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var getByGlossaryIdSql = "SELECT * FROM \"PartsOfSpeech\" WHERE \"LocaleID\" IN " +
                        "(SELECT \"ID_SourceLocale\" FROM \"LocalizationProjects\" WHERE \"ID\" IN " +
                        "(SELECT \"ID_LocalizationProject\" FROM \"LocalizationProjectsGlossaries\" " +
                            "WHERE \"ID_Glossary\"=@GlossaryId))";
                    var getByGlossaryIdParam = new

                    {
                        GlossaryId = glossaryId
                    };
                    this.LogQuery(sql: getByGlossaryIdSql, param: getByGlossaryIdParam);
                    var partsOfSpeechForLocale = dbConnection
                        .Query<PartOfSpeech>(
                            sql: getByGlossaryIdSql,
                            param: getByGlossaryIdParam);
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
