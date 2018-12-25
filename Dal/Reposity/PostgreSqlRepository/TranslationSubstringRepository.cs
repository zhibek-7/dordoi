using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using SqlKata;
using Models.Interfaces.Repository;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class TranslationSubstringRepository : BaseRepository, ITranslationSubstringRepository
    {
        private PostgreSqlNativeContext context;

        public TranslationSubstringRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Добавляет новую фразу
        /// </summary>
        /// <param name="item">Новая фраза</param>
        /// <returns>Кол-во добавленных фраз</returns>
        public Task<int> AddAsync(TranslationSubstring item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получает все фразы
        /// </summary>
        /// <returns>Список фраз</returns>
        public async Task<IEnumerable<TranslationSubstring>> GetAllAsync()
        {
            var query = "SELECT * FROM \"TranslationSubstrings\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> strings = await dbConnection.QueryAsync<Models.DatabaseEntities.TranslationSubstring>(query);
                    dbConnection.Close();
                    return strings;
                }
            }
            catch(Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
            
        }

        /// <summary>
        /// Получает запись с конкретным id
        /// </summary>
        /// <param name="id">id необходимой фразы</param>
        /// <returns>Запись с необходимым id</returns>
        public async Task<TranslationSubstring> GetByIDAsync(int id)
        {
            var query = "SELECT * " +
                        "FROM \"TranslationSubstrings\" " +
                        "WHERE \"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var foundedString = await dbConnection.QuerySingleAsync<Models.DatabaseEntities.TranslationSubstring>(query, new { Id = id });
                    dbConnection.Close();
                    return foundedString;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }            
        }


        /// <summary>
        /// Фильтрует список строк по определенной фразе
        /// </summary>
        /// <param name="filtredString">фраза по которой происходит фильтрация</param>
        /// <param name="filtredListOfStrings">список строк среди которых происходит фильтрация</param>
        /// <returns>список строк содержащихся в списке строк </returns>
        public async Task<IEnumerable<TranslationSubstring>> FilterByString(string filtredString , IEnumerable<TranslationSubstring> filtredListOfStrings)
        {
            var query = "";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> filtredStrings = await dbConnection.QueryAsync<TranslationSubstring>(query);
                    dbConnection.Close();
                    return filtredStrings;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
        }


        /// <summary>
        /// Получает записи из определенного и открытых проектов
        /// </summary>
        /// <param name="fileId">id определенного проекта</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationSubstring>> GetStringsInVisibleAndCurrentProjectdAsync(int projectId)
        {
            var query = "SELECT * " +
                        "FROM \"TranslationSubstrings\" AS TS " +
                        "INNER JOIN \"Files\" AS F ON TS.\"ID_FileOwner\" = F.\"ID\" " +
                        "INNER JOIN \"LocalizationProjects\" AS LP " +
                        "WHERE LP.\"ID\" = @Id " +
                        "OR LP.\"Visibility\" = true ";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> strings = await dbConnection.QueryAsync<TranslationSubstring>(query, new { Id = projectId });
                    dbConnection.Close();
                    return strings;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
        }


        /// <summary>
        /// Получает записи из определенного файла по id файла
        /// </summary>
        /// <param name="fileId">id файла</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationSubstring>> GetStringsByFileIdAsync(int fileId)
        {
            var query = "SELECT TS.\"SubstringToTranslate\" AS \"SubstringToTranslate\", TS.\"Description\" AS \"Description\", " +
                        "TS.\"Context\" AS \"Context\", TS.\"TranslationMaxLength\" AS \"TranslationMaxLength\"," +
                        "TS.\"ID_FileOwner\" AS \"ID_FileOwner\", TS.\"Value\" AS \"Value\"," +
                        "TS.\"PositionInText\" AS \"PositionInText\", TS.\"ID\" AS \"ID\" " +
                        "FROM \"TranslationSubstrings\" AS TS " +
                        "INNER JOIN \"Files\" AS F ON TS.\"ID_FileOwner\" = F.\"ID\" " +
                        "WHERE F.\"ID\" = @Id";
            
            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<TranslationSubstring> stringsInFile = await dbConnection.QueryAsync<TranslationSubstring>(query, new { Id = fileId });
                    dbConnection.Close();
                    return stringsInFile;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            using (var dbConnection = this.context.Connection)
            {
                var query = new Query("TranslationSubstrings")
                    .Where("ID", id)
                    .AsDelete();

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                dbConnection.Open();
                await dbConnection.ExecuteAsync(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
                dbConnection.Close();

                return true;
            }
        }

        public async Task<bool> UpdateAsync(Models.DatabaseEntities.TranslationSubstring item)
        {
            using (var dbConnection = this.context.Connection)
            {
                var query = new Query("TranslationSubstrings")
                    .Where("ID", item.ID)
                    .AsUpdate(item);

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                dbConnection.Open();
                await dbConnection.ExecuteAsync(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
                dbConnection.Close();

                return true;
            }
        }

        public static Dictionary<string, string> SortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "ID" },
            { "substringtotranslate", "SubstringToTranslate" },
            { "description", "Description" },
            { "context", "Context" },
            { "translationmaxlength", "TranslationMaxLength" },
            { "id_fileowner", "ID_FileOwner" },
            { "value", "Value" },
            { "positionintext", "PositionInText" },
            { "outdated", "Outdated" },
        };

        public async Task<IEnumerable<TranslationSubstring>> GetByProjectIdAsync(
            int projectId,
            int offset,
            int limit,
            int? fileId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            if (sortBy == null || !sortBy.Any())
            {
                sortBy = new[] { "id" };
            }

            using (var dbConnection = this.context.Connection)
            {
                var query = this.GetByProjectIdQuery(
                    projectId: projectId,
                    fileId: fileId,
                    searchString: searchString);

                query = this.ApplyPagination(
                    query: query,
                    offset: offset,
                    limit: limit);

                query = this.ApplySorting(
                    query: query,
                    columnNamesMappings: TranslationSubstringRepository.SortColumnNamesMapping,
                    sortBy: sortBy,
                    sortAscending: sortAscending);

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                dbConnection.Open();
                var translationSubstrings = await dbConnection.QueryAsync<TranslationSubstring>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
                dbConnection.Close();

                return translationSubstrings;
            }
        }

        public async Task<int> GetByProjectIdCountAsync(
            int projectId,
            int? fileId = null,
            string searchString = null)
        {
            using (var dbConnection = this.context.Connection)
            {
                var query = this.GetByProjectIdQuery(
                    projectId: projectId,
                    fileId: fileId,
                    searchString: searchString);
                query = query.AsCount();

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                dbConnection.Open();
                var count = await dbConnection.ExecuteScalarAsync<int>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
                dbConnection.Close();

                return count;
            }
        }

        private Query GetByProjectIdQuery(
            int projectId,
            int? fileId = null,
            string searchString = null)
        {
            var query = new Query("TranslationSubstrings")
                .Select(
                    "ID",
                    "SubstringToTranslate",
                    "Description",
                    "Context",
                    "TranslationMaxLength",
                    "ID_FileOwner",
                    "Value",
                    "PositionInText",
                    "Outdated"
                );

            if (fileId != null)
            {
                query = query.Where("ID_FileOwner", fileId);
            }
            else
            {
                query = query
                    .WhereIn("ID_FileOwner",
                        new Query("Files")
                            .Select("ID")
                            .Where("ID_LocalizationProject", projectId));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchPattern = $"%{searchString}%";
                query = query.WhereLike("Value", searchPattern);
            }

            return query;
        }

        public async Task<IEnumerable<Locale>> GetLocalesForStringAsync(int translationSubstringId)
        {
            using (var dbConnection = this.context.Connection)
            {
                var query =
                    new Query("Locales")
                    .WhereIn("ID",
                        new Query("TranslationsubStringsLocales")
                        .Select("Id_Locales")
                        .Where("Id_TranslationSubStrings", translationSubstringId));

                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);

                dbConnection.Open();
                var locales = await dbConnection.QueryAsync<Locale>(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings
                    );
                dbConnection.Close();

                return locales;
            }
        }

        public async Task DeleteTranslationLocalesAsync(int translationSubstringId)
        {
            using (var dbConnection = this.context.Connection)
            {
                dbConnection.Open();
                var query =
                    new Query("TranslationsubStringsLocales")
                    .Where("Id_TranslationSubStrings", translationSubstringId)
                    .AsDelete();
                var compiledQuery = this._compiler.Compile(query);
                this.LogQuery(compiledQuery);
                await dbConnection.ExecuteAsync(
                    sql: compiledQuery.Sql,
                    param: compiledQuery.NamedBindings);
                dbConnection.Close();
            }
        }

        public async Task AddTranslationLocalesAsync(int translationSubstringId, IEnumerable<int> localesIds)
        {
            using (var dbConnection = this.context.Connection)
            {
                dbConnection.Open();
                foreach (var localeId in localesIds)
                {
                    var sql =
                        "INSERT INTO \"TranslationsubStringsLocales\" " +
                        "(" +
                        "\"Id_TranslationSubStrings\", " +
                        "\"Id_Locales\"" +
                        ") VALUES " +
                        "(" +
                        "@Id_TranslationSubStrings, " +
                        "@Id_Locales" +
                        ")";
                    var param = new { Id_TranslationSubStrings = translationSubstringId, Id_Locales = localeId };
                    this.LogQuery(sql, param);
                    await dbConnection.ExecuteAsync(
                        sql: sql,
                        param: param);
                }
                dbConnection.Close();
            }
        }

    }
}
