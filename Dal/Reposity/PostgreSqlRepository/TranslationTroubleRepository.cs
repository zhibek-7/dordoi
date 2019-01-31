using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DAL.Context;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Npgsql;

namespace DAL.Reposity.PostgreSqlRepository
{
    public enum TroubleTypes
    {
        SpecSymbol = 1, //1	Спец символы в переводе                                                                            
        FirstLetter //2	Наачало предложения не с заглавной буквы                                                                                  
    }

    public class TranslationTroubleRepository : BaseRepository, ITranslationTroubleRepository
    {
        public TranslationTroubleRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<int> AddAsync(TranslationTrouble item)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "INSERT INTO translations_troubles" +
                               " (id_trouble, trouble, id_translation) " +
                               "VALUES (@ID_Trouble, @Trouble, @ID_Translation)";
                    var _params = new
                    {
                        item.ID_Trouble,
                        item.Trouble,
                        item.ID_Translation
                    };
                    LogQuery(_sql, _params);
                    var insertedId = await dbConnection.ExecuteScalarAsync<int>(_sql, _params);
                    return insertedId;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    string SQLQuery = "DELETE FROM translations_troubles WHERE id = @Id";
                    var param = new { id };
                    this.LogQuery(SQLQuery, param);
                    dbConnection.Execute(SQLQuery, param);
                }

                return true;
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.RemoveAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.RemoveAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TranslationTrouble item)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "UPDATE translations_troubles" +
                               " SET id_trouble = @ID_Trouble, trouble = @Trouble, id_translation = @ID_Translation" +
                               " WHERE id = @ID";
                    var _params = new
                    {
                        item.ID_Trouble,
                        item.Trouble,
                        item.ID_Translation,
                        item.ID
                    };
                    LogQuery(_sql, _params);
                    await dbConnection.ExecuteScalarAsync<int>(_sql, _params);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.UpdateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

        public async Task<TranslationTrouble> GetByIDAsync(int id)
        {
            try
            {
                string sqlQery = "SELECT * FROM translations_troubles WHERE Id = @Id";
                TranslationTrouble trouble = null;
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(sqlQery, param);
                    trouble = dbConnection.Query<TranslationTrouble>(sqlQery, param).FirstOrDefault();
                }
                return trouble;
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<IEnumerable<TranslationTrouble>> GetAllAsync()
        {
            try
            {
                string sqlQery = "SELECT * FROM translations_troubles";
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sqlQery);
                    IEnumerable<TranslationTrouble> troubles = dbConnection.Query<TranslationTrouble>(sqlQery).ToList();
                    return troubles;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        public async Task<IEnumerable<TranslationTrouble>> GetByTranslationIdAsync(int translationId)
        {
            try
            {
                string sqlQery = "SELECT * FROM translations_troubles WHERE id_translation = @Id";
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { translationId };
                    this.LogQuery(sqlQery, param);
                    IEnumerable<TranslationTrouble> troubles = dbConnection.Query<TranslationTrouble>(sqlQery, param).ToList();
                    return troubles;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.GetByTranslationIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(TranslationTroubleRepository)}.{nameof(TranslationTroubleRepository.GetByTranslationIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }
    }
}
