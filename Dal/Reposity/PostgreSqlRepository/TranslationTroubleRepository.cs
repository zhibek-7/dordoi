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
                    var _sql = "INSERT INTO \"TranslationsTroubles\"" +
                               " (\"ID_Trouble\", \"Trouble\", \"ID_Translation\") " +
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
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TranslationTroubleRepository.AddAsync ", exception);
                return 0;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    string SQLQuery = "DELETE FROM \"TranslationsTroubles\" WHERE \"ID\" = @Id";
                    dbConnection.Execute(SQLQuery, new { id });
                }

                return true;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TranslationTroubleRepository.RemoveAsync ", exception);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TranslationTrouble item)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var _sql = "UPDATE \"TranslationsTroubles\"" +
                               " SET \"ID_Trouble\" = @ID_Trouble, \"Trouble\" = @Trouble, \"ID_Translation\" = @ID_Translation" +
                               " WHERE \"ID\" = @ID";
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
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TranslationTroubleRepository.UpdateAsync ", exception);
                return false;
            }
        }

        public async Task<TranslationTrouble> GetByIDAsync(int id)
        {
            try
            {
                TranslationTrouble trouble = null;
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    trouble = dbConnection.Query<TranslationTrouble>("SELECT * FROM \"TranslationsTroubles\" WHERE Id = @Id", new { id }).FirstOrDefault();
                }
                return trouble;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TranslationTroubleRepository.GetByIDAsync ", exception);
                return null;
            }
        }

        public async Task<IEnumerable<TranslationTrouble>> GetAllAsync()
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    IEnumerable<TranslationTrouble> troubles = dbConnection.Query<TranslationTrouble>("SELECT * FROM \"TranslationsTroubles\"").ToList();
                    return troubles;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TranslationTroubleRepository.GetAllAsync ", exception);
                return null;
            }
        }

        public async Task<IEnumerable<TranslationTrouble>> GetByTranslationIdAsync(int translationId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    IEnumerable<TranslationTrouble> troubles = dbConnection.Query<TranslationTrouble>("SELECT * FROM \"TranslationsTroubles\" WHERE \"ID_Translation\" = @Id", new { translationId }).ToList();
                    return troubles;
                }
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn("Ошибка в TranslationTroubleRepository.GetByTranslationIdAsync ", exception);
                return null;
            }
        }
    }
}
