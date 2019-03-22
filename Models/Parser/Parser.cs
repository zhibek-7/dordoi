using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Models.DatabaseEntities;
using Utilities.Logs;

namespace Models.Parser
{
    /// <summary>
    /// Класс, реализующий логику распарсивания в виде функций-парсеров, обрабатывающих объекты класса <see cref="File"/> и возвращающих списки <see cref="List{}"/> объектов класса <see cref="TranslationSubstring"/>
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Поле, предназначенное для логирования класса
        /// </summary>
        protected readonly ILogTools _logger = new LogTools();
        //protected readonly ILogTools _loggerError = new ExceptionLog();

        /// <summary>
        /// Делегат (маска) функции-парсера
        /// </summary>
        private delegate List<TranslationSubstring> ParseFunction(File file);

        /// <summary>
        /// Словарь функций-парсеров, где ключом служит расширение в виде записи <see cref="string"/>, а значением - соответствующий объект делегата <see cref="ParseFunction"/>
        /// </summary>
        private Dictionary<string, ParseFunction> ParseFunctions;

        /// <summary>
        /// Инициализирует объект класса <see cref="Parser"/>
        /// </summary>
        public Parser()
        {
            ParseFunctions = new Dictionary<string, ParseFunction>()
            {
                {"po", ParseAsPo_Pot },
                {"pot", ParseAsPo_Pot },
                {"properties", ParseAsProperties },
                {"ini", ParseAsIni },
                {"json", ParseAsJson },
                {"strings", ParseAsStrings },
                {"csv", ParseAsCsv },
                {"xml", ParseAsXml },
                {"php", ParseAsPhp },
                {"resx", ParseAsResx },
                {"string", ParseAsString },
                {"txt", ParseAsTxt },
                {"rc", ParseAsRc },
                {"yml", ParseAsYml_Yaml },
                {"yaml", ParseAsYml_Yaml },
                {"docx", ParseAsDocx },
                {"odt", ParseAsOdt },
                {"xlsx", ParseAsXlsx },
                {"ods", ParseAsOds }
            };
        }

        /// <summary>
        /// Метод, предназначенный для распасрсивания файла всеми представленными в словаре <see cref="ParseFunctions"/> функциями-парсерами
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Cловарь, в котором ключ <see cref="string"/> - расширение, а значение - выделенный соответствующим парсером список <see cref="List{}"/> объектов класса <see cref="TranslationSubstring"/></returns>
        public Dictionary<string, List<TranslationSubstring>> UseAllParsers(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("Попытка распарсивания файла {0} всеми доступными парсерами", file.name_text));
            var ans = new Dictionary<string, List<TranslationSubstring>>();
            foreach (var pf in ParseFunctions.GroupBy(v => v.Value)) ans.Add(pf.First().Key, pf.Key(file));
            var max = ans.First(a => a.Value.Count == ans.Values.Max(v => v.Count));
            _logger.WriteLn("Parser: " + string.Format("Для файла {0} наиболее релевантен '{1}'-парсер (обнаружено записей: {2})", file.name_text, max.Key, max.Value.Count));
            return ans;
        }

        /// <summary>
        /// Метод, предназначенный для распарсивания файла: функция-парсер подбирается в зависимости от значения аргумента <paramref name="extension"/>
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <param name="extension">Необязательный аргумент: если указан, принудительно используется соответствующий парсер, в противном случае - парсер подбирается автоматически</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/>, выделенных из файла выбранным (или автоматически подобранным) парсером</returns>
        public List<TranslationSubstring> Parse(File file, string extension = "")
        {
            _logger.WriteLn("Parser: "
            + string.Format("Попытка распарсивания файла {0}", file.name_text));
            string ext = string.IsNullOrEmpty(extension) ? file.name_text.Split('.').Last().ToLower() : extension;
            var ts = new List<TranslationSubstring>();
            if (ParseFunctions.ContainsKey(ext)) ts = ParseFunctions[ext](file);
            else
                throw new ParserException(ParserException.ParserExceptionTypes.WrongExtension);
            if (ts.Count == 0) throw new ParserException(ParserException.ParserExceptionTypes.NoElements);
            _logger.WriteLn(string.Format("Файл {0} успешно распарсен{1}", file.name_text, string.IsNullOrEmpty(extension) ? string.Empty : string.Format(" (принудительно применен парсер для расширения '{0}')", ext)));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'po/pot'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsPo_Pot(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'po/pot'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?:msgctxt\\s+\"([^\"]*)\"\\s+)?msgid\\s+\"([^\"]*)\"\\s+(msgid_plural\\s+\"([^\"]*)\"\\s+)?((?:msgstr(?:\\[\\d+\\])?\\s+\"[^\"]*\"\\s*)+)";
            string subpattern = "msgstr(?:\\[\\d+\\])?\\s+\"([^\"]*)\"";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches)
            {
                var context = m.Groups[1].Value;
                var original = m.Groups[string.IsNullOrEmpty(m.Groups[3].Value) ? 2 : 4].Value;
                var matches_trans = Regex.Matches(m.Groups[5].Value, subpattern);
                foreach (Match m_t in matches_trans) ts.Add(new TranslationSubstring(original, context, file.id, m_t.Groups[1].Value, m.Groups[5].Index + m_t.Groups[1].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'po/pot'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'properties'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsProperties(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'properties'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "^\\s*(?!#)(?:.|(?<=\\\\)\\n)+";
            string subpattern = "([^=\\n]+)=(.*)";
            var matches = Regex.Matches(file.original_full_text, pattern, RegexOptions.Multiline);
            foreach (Match m in matches)
            {
                var m_sp = Regex.Match(m.Value, subpattern, RegexOptions.Singleline);
                ts.Add(new TranslationSubstring(m_sp.Groups[2].Value, m_sp.Groups[1].Value, file.id, m_sp.Groups[2].Value, m.Index + m_sp.Groups[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'properties'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'ini'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsIni(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'ini'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "^\\s*(?!;)(?:.|(?<=\\\\)\\n)+";
            string subpattern = "([^=\\n]+)=\\s*\"((?:[^\"]|(?<=\\\\)\")+)\"";
            var matches = Regex.Matches(file.original_full_text, pattern, RegexOptions.Multiline);
            foreach (Match m in matches)
            {
                var m_sp = Regex.Match(m.Value, subpattern, RegexOptions.Singleline);
                ts.Add(new TranslationSubstring(m_sp.Groups[2].Value, m_sp.Groups[1].Value, file.id, m_sp.Groups[2].Value, m.Index + m_sp.Groups[2].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'ini'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'json'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsJson(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'json'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(,|\\{|\\[|:)\\s*\"((?:[^\r\n\"]|(?<=\\\\)[\r\n\"]|(?<=\r)\n)*)\"(\\s*[\\}\\]])*";
            var matches = Regex.Matches(file.original_full_text, pattern);
            List<string> context_parts = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                bool isContextPart = false;
                if (matches[i].Groups[1].Value == "{") context_parts.Add(matches[i].Groups[2].Value);
                if (matches[i].Groups[1].Value == "," && Regex.IsMatch(matches[i + 1].Groups[1].Value, "[:\\{\\[]")) { context_parts.RemoveAt(context_parts.Count - 1); context_parts.Add(matches[i].Groups[2].Value); isContextPart = true; }
                if (!isContextPart && Regex.IsMatch(matches[i].Groups[1].Value, "[:\\[,]"))
                {
                    string context = string.Empty;
                    for (int j = 0; j < context_parts.Count; j++) context += context_parts[j] + "->";
                    ts.Add(new TranslationSubstring(matches[i].Groups[2].Value, context, file.id, matches[i].Groups[2].Value, matches[i].Groups[2].Index));
                }
                foreach (var m in Regex.Matches(matches[i].Groups[3].Value, "[\\}\\]]")) context_parts.RemoveAt(context_parts.Count - 1);
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'json'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'strings'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsStrings(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'strings'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'strings'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'csv'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsCsv(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'csv'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "^\\s*(?!#)(\"(?:\"\"|[^\"])*\"[,;]*)+";
            string subpattern = "\"(?:\"\"|[^\"])*\"";
            var matches = Regex.Matches(file.original_full_text, pattern, RegexOptions.Multiline);
            foreach (Match m in matches)
            {
                var matches_sp = Regex.Matches(m.Value, subpattern);
                if (matches_sp.Count == 2) ts.Add(new TranslationSubstring(matches_sp[0].Value, string.Empty, file.id, matches_sp[1].Value, m.Index + matches_sp[1].Index));
                if (matches_sp.Count >= 3) ts.Add(new TranslationSubstring(matches_sp[1].Value, matches_sp[0].Value, file.id, matches_sp[2].Value, m.Index + matches_sp[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'csv'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'xml'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsXml(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'xml'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string simpleRowPattern = "<string\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>([^<]*)</string\\s*>";
            string arrayPattern = "<string-array\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>((?:(?!</string-array).)*)</string-array\\s*>";
            string arrayItemPattern = "<item\\s*>((?:(?!</item).)*)</item\\s*>";
            var matches = Regex.Matches(file.original_full_text, simpleRowPattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            matches = Regex.Matches(file.original_full_text, arrayPattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                string context = m.Groups[1].Value;
                var itemMatches = Regex.Matches(m.Groups[2].Value, arrayItemPattern, RegexOptions.Singleline);
                foreach (Match m2 in itemMatches) ts.Add(new TranslationSubstring(m2.Groups[1].Value, m.Groups[1].Value, file.id, m2.Groups[1].Value, m.Groups[2].Index + m2.Groups[1].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'xml'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'php'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsPhp(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'php'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern_array = "(array\\s*[(]|[[]|=>|(?:[)]|[]])?\\s*,)\\s*('((?:(?<=\\\\)'|[^'])*)'|\\d+)";
            string pattern_array_subpattern = "(?:[)]|[]])\\s*,\\s*$";
            string pattern_define = "DEFINE\\s*[(]\\s*\"((?:(?<=\\\\)\"|[^\"])*)\"\\s*,\\s*\"((?:(?<=\\\\)\"|[^\"])*)\"\\s*[)]\\s*;";
            string pattern_array2 = "([$]\\w+)\\s*=\\s*array\\s*[(]\\s*((?:\"(?:(?<=\\\\)\"|[^\"])*\"\\s*=>\\s*\"(?:(?<=\\\\)\"|[^\"])*\"\\s*,*\\s*)*)\\s*[)]\\s*;";
            string pattern_array2_subpattern = "\"((?:(?<=\\\\)\"|[^\"])*)\"\\s*=>\\s*\"((?:(?<=\\\\)\"|[^\"])*)\"";
            string pattern_array2_alt = "([$]\\w+)\\s*[[]\\s*[\"']((?:(?<=\\\\)[\"']|[^\"'])*)[\"']\\s*[]]\\s*=\\s*[\"']((?:(?<=\\\\)[\"']|[^\"'])*)[\"']";

            var matches = Regex.Matches(file.original_full_text, pattern_array);
            var contextParts = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                string context = "array";
                if (Regex.IsMatch(matches[i].Groups[1].Value, "=>"))
                {
                    for (int j = 0; j < contextParts.Count; j++) context += string.Format("[{0}]", contextParts[j]);
                    ts.Add(new TranslationSubstring(matches[i].Groups[3].Value, context, file.id, matches[i].Groups[3].Value, matches[i].Groups[3].Index));
                    contextParts.RemoveAt(contextParts.Count - 1);
                }
                else
                {
                    if (contextParts.Count > 0 && Regex.IsMatch(matches[i].Groups[1].Value, pattern_array_subpattern)) contextParts.RemoveAt(contextParts.Count - 1);
                    contextParts.Add(matches[i].Groups[2].Value);
                }
            }

            matches = Regex.Matches(file.original_full_text, pattern_define);
            foreach (Match m in matches) ts.Add(new TranslationSubstring(m.Groups[1].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));

            matches = Regex.Matches(file.original_full_text, pattern_array2);
            foreach (Match m in matches)
            {
                var matches_sp = Regex.Matches(m.Groups[2].Value, pattern_array2_subpattern);
                foreach (Match m_sp in matches_sp) ts.Add(new TranslationSubstring(m_sp.Groups[1].Value, string.Format("{0}[{1}]", m.Groups[1].Value, m_sp.Groups[1].Value), file.id, m_sp.Groups[2].Value, m.Groups[2].Index + m_sp.Groups[2].Index));
            }

            matches = Regex.Matches(file.original_full_text, pattern_array2_alt);
            foreach (Match m in matches) ts.Add(new TranslationSubstring(m.Groups[2].Value, string.Format("{0}[{1}]", m.Groups[1].Value, m.Groups[2].Value), file.id, m.Groups[3].Value, m.Groups[3].Index));

            _logger.WriteLn("Parser: " + string.Format("Парсер 'php'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'resx'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsResx(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'resx'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "<data(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*>(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*<value\\s*>([^<]*)</value\\s*>\\s*(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*</data\\s*>";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'resx'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'string'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsString(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'string'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'string'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'txt'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsTxt(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'txt'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(.+)\r?\n?";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[1].Value, string.Empty, file.id, m.Groups[1].Value, m.Groups[1].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'txt'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'rc'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsRc(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'rc'", file.name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "\\s*(\\d+)\\s*,\\s*\"((?:[^\"]|(?<=\\\\)\")*)\"\\s*";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'rc'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'yml/yaml'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsYml_Yaml(File file)
        {
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'yml/yaml'", file.name_text));
            var ts = new List<TranslationSubstring>();
            var pattern = "^([ ]*)([^#].*\n)";
            var rowpattern = "((?:(?:[^:]|:(?![ ]))+|['\"].*['\"])):[ ]?";
            var matches = Regex.Matches(file.original_full_text, pattern, RegexOptions.Multiline);
            var context_parts = new List<string>();
            string context = string.Empty;
            for (int i = 0; i < matches.Count; i++)
            {
                var m_row = Regex.Matches(matches[i].Groups[2].Value, rowpattern);
                if (m_row.Count > 0)
                {
                    var indent = matches[i].Groups[1].Length;
                    context_parts = context_parts.Take(indent / 2).ToList();
                    var context_row = string.Empty;
                    int n = -1;
                    while (n + 1 < m_row.Count && !Regex.IsMatch(m_row[n + 1].Value, "^[ ]*{[ ]*"))
                    {
                        context_row += "[" + m_row[n + 1].Groups[1].Value + "]";
                        n++;
                    }
                    context_parts.Add(context_row);
                    context = string.Empty;
                    foreach (var s in context_parts) context += s;
                    var nodetext_index = m_row[n].Index + m_row[n].Length;
                    string nodetext = matches[i].Groups[2].Value.Substring(nodetext_index);
                    nodetext_index += matches[i].Groups[2].Index;
                    if (Regex.IsMatch(nodetext, "\\S+"))
                    {
                        if (Regex.IsMatch(nodetext, "^[>|{]\\s+"))
                        {
                            var value = nodetext;
                            while (i + 1 < matches.Count && matches[i + 1].Groups[1].Length > indent)
                            {
                                value += matches[i + 1].Value;
                                i++;
                            }
                            value = value.Substring(0, value.Length - 1);
                            ts.Add(new TranslationSubstring(value, context, file.id, value, nodetext_index));
                            continue;
                        }
                        nodetext = nodetext.Substring(0, nodetext.Length - 1);
                        ts.Add(new TranslationSubstring(nodetext, context, file.id, nodetext, nodetext_index));
                    }
                }
                else
                {
                    var value = matches[i].Groups[2].Value.Substring(0, matches[i].Groups[2].Value.Length - 1);
                    ts.Add(new TranslationSubstring(value, context, file.id, value, matches[i].Groups[2].Index));
                }
            }
            _logger.WriteLn("Parser: " + string.Format("Парсер 'yml/yaml'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'docx'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsDocx(File file)
        {
            //Предполагается, что в качестве original_full_text будет выступать document.xml из соответствующего архива docx
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'docx'", file.name_text));
            //var file = DownloadsFolderPath + @"\Краткая инструкция VS2017.docx";
            //var path = Path.GetTempPath() + Guid.NewGuid();
            //using (var zf = ZipFile.Open(file, ZipArchiveMode.Read)) zf.ExtractToDirectory(path);
            //var docPath = path + @"\word\document.xml";
            //var text = File.ReadAllText(docPath);
            var ts = new List<TranslationSubstring>();
            var pattern_p = "<w:p(?:\\s*\\w+:?\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>(((?<!</w:p>).)*)</w:p>";
            var pattern_t = ">([^>]+)<";
            var prs = Regex.Matches(file.original_full_text, pattern_p);
            int n = 0;
            foreach (Match m_p in prs)
            {
                var txs = Regex.Matches(m_p.Groups[1].Value, pattern_t);
                foreach (Match m_t in txs)
                {
                    var pos = m_p.Groups[1].Index + m_t.Groups[1].Index;
                    ts.Add(new TranslationSubstring(m_t.Groups[1].Value, string.Empty, file.id, m_t.Groups[1].Value, pos, n));
                }
                n++;
            }
            //Directory.Delete(path, true);
            _logger.WriteLn("Parser: " + string.Format("Парсер 'docx'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'odt'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsOdt(File file)
        {
            //Предполагается, что в качестве original_full_text будет выступать content.xml из соответствующего архива odt
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'odt'", file.name_text));
            //var file = DownloadsFolderPath + @"\Краткая инструкция VS2017.odt";
            //var path = Path.GetTempPath() + Guid.NewGuid();
            //using (var zf = ZipFile.Open(file, ZipArchiveMode.Read)) zf.ExtractToDirectory(path);
            //var docPath = path + @"\content.odt";
            //var text = File.ReadAllText(docPath);
            var ts = new List<TranslationSubstring>();
            var pattern_p = "<text:(?<type>p|h)(?:\\s*\\w+:?[\\w-]+=\"[^\"]*\"\\s*)*(>((?<!</text:\\k<type>>).)*<)/text:\\k<type>>";
            var pattern_t = ">([^>]+)<";
            var prs = Regex.Matches(file.original_full_text, pattern_p);
            int n = 0;
            foreach (Match m_p in prs)
            {
                var txs = Regex.Matches(m_p.Groups[1].Value, pattern_t);
                foreach (Match m_t in txs)
                {
                    var pos = m_p.Groups[1].Index + m_t.Groups[1].Index;
                    ts.Add(new TranslationSubstring(m_t.Groups[1].Value, string.Empty, file.id, m_t.Groups[1].Value, pos, n));
                }
                n++;
            }
            //Directory.Delete(path, true);
            _logger.WriteLn("Parser: " + string.Format("Парсер 'odt'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'xlsx'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsXlsx(File file)
        {
            //Предполагается, что в качестве original_full_text будет выступать xl\sharedStrings.xml из соответствующего архива xlsx
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'xlsx'", file.name_text));
            //var file = DownloadsFolderPath + @"\Input\Пример для парсенга.xlsx";
            //var path = Path.GetTempPath() + Guid.NewGuid();
            //using (var zf = ZipFile.Open(file, ZipArchiveMode.Read)) zf.ExtractToDirectory(path);
            //var docPath = path + @"\xl\sharedStrings.xml";
            //var text = File.ReadAllText(docPath);
            var ts = new List<TranslationSubstring>();
            var pattern = "<t(?:\\s*\\w+:?[\\w-]+=\"[^\"]*\"\\s*)*>(((?<!</t>).)*)</t>";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches) ts.Add(new TranslationSubstring(m.Groups[1].Value, string.Empty, file.id, m.Groups[1].Value, m.Groups[1].Index));
            //Directory.Delete(path, true);
            _logger.WriteLn("Parser: " + string.Format("Парсер 'xlsx'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'ods'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsOds(File file)
        {
            //Предполагается, что в качестве original_full_text будет выступать content.xml из соответствующего архива ods
            _logger.WriteLn("Parser: " + string.Format("К файлу {0} применяется парсер для файлов с расширением 'ods'", file.name_text));
            //var file = DownloadsFolderPath + @"\Input\Пример для парсенга.ods";
            //var path = Path.GetTempPath() + Guid.NewGuid();
            //using (var zf = ZipFile.Open(file, ZipArchiveMode.Read)) zf.ExtractToDirectory(path);
            //var docPath = path + @"\content.xml";
            //var text = File.ReadAllText(docPath);
            var ts = new List<TranslationSubstring>();
            var pattern = "<text:p(?:\\s*\\w+:?[\\w-]+=\"[^\"]*\"\\s*)*>(((?<!</text:p>).)*)</text:p>";
            var matches = Regex.Matches(file.original_full_text, pattern);
            foreach (Match m in matches) ts.Add(new TranslationSubstring(m.Groups[1].Value, string.Empty, file.id, m.Groups[1].Value, m.Groups[1].Index));
            //Directory.Delete(path, true);
            _logger.WriteLn("Parser: " + string.Format("Парсер 'ods'-файлов обнаружил в файле {0} записей: {1}", file.name_text, ts.Count));
            return ts;
        }
    }
}
