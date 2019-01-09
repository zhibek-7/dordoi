using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlKata;
using SqlKata.Compilers;
using Utilities.Logs;

namespace DAL.Reposity.PostgreSqlRepository
{
    public abstract class BaseRepository
    {

        protected readonly ILogTools _logger = new LogTools();
        protected readonly ILogTools _loggerError = new ExceptionLog();

        protected readonly Compiler _compiler = new PostgresCompiler();

        protected void LogQuery(string sql)
        {
            this._logger.WriteLn($"Query {sql}");
        }

        protected void LogQuery(string sql, object param)
        {
            this._logger.WriteLn($"Query {sql}, param: {param}");
        }

        protected void LogQuery(SqlResult sqlResult)
        {
            if (sqlResult.NamedBindings.Any())
            {
                this._logger.WriteLn($"Query {sqlResult.Sql}, param: {this.DictionaryToString(sqlResult.NamedBindings)}");
            }
            else
            {
                this._logger.WriteLn($"Query {sqlResult.Sql}");
            }
        }

        private string DictionaryToString(Dictionary<string, object> dictionary)
        {
            var stringBuilder =
                dictionary.SkipLast(1)
                .Aggregate(
                    seed: new StringBuilder("{ "),
                    func: (seed, pair) => seed.Append($"{pair.Key} = {pair.Value}, "));
            return dictionary.TakeLast(1)
                .Select(pair => stringBuilder.Append($"{pair.Key} = {pair.Value} }}"))
                .FirstOrDefault()?.ToString() ?? "null";
        }

        protected Query ApplyPagination(Query query, int offset, int limit)
        {
            if (offset > 0)
            {
                query = query.Skip(offset);
            }
            if (limit > 0)
            {
                query = query.Take(limit);
            }
            return query;
        }

        protected Query ApplySorting(Query query, Dictionary<string, string> columnNamesMappings, string[] sortBy, bool sortAscending)
        {
            if (sortBy.Any())
            {
                var columnNamesToSort = sortBy
                    .Where(x => x != null)
                    .Select(x => x.ToLower())
                    .Distinct()
                    .Where(x => columnNamesMappings.ContainsKey(x))
                    .Select(x => columnNamesMappings[x])
                    .ToArray();
                if (columnNamesToSort.Any())
                    query = sortAscending ? query.OrderBy(columnNamesToSort) : query.OrderByDesc(columnNamesToSort);
            }
            return query;
        }

    }
}
