using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Logs;

namespace DAL.Reposity.PostgreSqlRepository
{
    public abstract class BaseRepository
    {

        protected readonly ILogTools _logger = new LogTools();
        protected readonly ILogTools _loggerError = new ExceptionLog();

        protected void LogQuery(string sql)
        {
            this._logger.WriteLn($"Query {sql}");
        }

        protected void LogQuery(string sql, object param)
        {
            this._logger.WriteLn($"Query {sql}, param: {param}");
        }

        protected string DictionaryToString(Dictionary<string, object> dictionary)
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

    }
}
