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

        /// <summary>
        /// Делегат (маска) функции-парсера
        /// </summary>
        private delegate List<TranslationSubstring> ParseFunction(File file);

        /// <summary>
        /// Словарь функций-парсеров, где ключом служит расширение в виде записи <see cref="string"/>, а значением - соответствующий объект делегата <see cref="ParseFunction"/>
        /// </summary>
        private Dictionary<string, ParseFunction> ParseFunctions;

        //public string AllowedExtensionsPattern { get; private set; }
        //public string PoPattern { get; private set; }
        //public string PropertiesPattern { get; private set; }
        //public string JsonPattern { get; private set; }
        //public string StringsPattern { get; private set; }
        //public string CsvPattern { get; private set; }
        //public string XmlSimpleRowPattern { get; private set; }
        //public string XmlArrayPattern { get; private set; }
        //public string XmlArrayItemPattern { get; private set; }
        //public string PhpArrayElementPattern { get; private set; }
        //public string PhpArrayEndPattern { get; private set; }
        //public string ResxPattern { get; private set; }
        //public string StringPattern { get; private set; }
        //public string TxtPattern { get; private set; }
        //public string RcPattern { get; private set; }

        //private ParserData PD = new ParserData();
        //public List<TranslationSubstring> TranslationSubstrings;

        /// <summary>
        /// Инициализирует объект класса <see cref="Parser"/>
        /// </summary>
        public Parser()
        {
            ParseFunctions = new Dictionary<string, ParseFunction>()
            {
                {"po", ParseAsPo },
                {"properties", ParseAsProperties },
                {"json", ParseAsJson },
                {"strings", ParseAsStrings },
                {"csv", ParseAsCsv },
                {"xml", ParseAsXml },
                {"php", ParseAsPhp },
                {"resx", ParseAsResx },
                {"string", ParseAsString },
                {"txt", ParseAsTxt },
                {"rc", ParseAsRc }
            };
            //this.AllowedExtensionsPattern = "(po|properties|json|strings|csv|xml|php|resx|string|txt|rc)";
            //this.PoPattern = "(?:msgctxt\\s+\"([^\"]*)\"\\s+)?msgid\\s+\"([^\"]*)\"\\s+msgstr\\s+\"([^\"]*)\"";
            //this.PropertiesPattern = "(.*)=(.*)\\s";
            //this.JsonPattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*:\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            //this.StringsPattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            //this.CsvPattern = "(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"(?:(?<=\")\"|[^\"])*(?<!\")\"";
            //this.XmlSimpleRowPattern = "<string\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>([^<]*)</string\\s*>";
            //this.XmlArrayPattern = "<string-array\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>((?:(?!</string-array).)*)</string-array\\s*>";
            //this.XmlArrayItemPattern = "<item\\s*>((?:(?!</item).)*)</item\\s*>";
            //this.PhpArrayElementPattern = "(array\\s*[(]|[[]|=>|(?:[)]|[]])?\\s*,)\\s*((?<!\\\\)'((?:(?<=\\\\)'|[^'])*)(?<!\\\\)'|\\d+)";
            //this.PhpArrayEndPattern = "(?:[)]|[]])\\s*,\\s*$";
            //this.ResxPattern = "<data(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*>(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*<value\\s*>([^<]*)</value\\s*>\\s*(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*</data\\s*>";
            //this.StringPattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            //this.TxtPattern = "(.+)\r?\n?";
            //this.RcPattern = "\\s*(\\d+)\\s*,\\s*\"((?:[^\"]|(?<=\\\\)\")*)\"\\s*";
        }

        /// <summary>
        /// Метод, предназначенный для распасрсивания файла всеми представленными в словаре <see cref="ParseFunctions"/> функциями-парсерами
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Cловарь, в котором ключ <see cref="string"/> - расширение, а значение - выделенный соответствующим парсером список <see cref="List{}"/> объектов класса <see cref="TranslationSubstring"/></returns>
        public Dictionary<string, List<TranslationSubstring>> UseAllParsers(File file)
        {
            _logger.WriteLn(string.Format("Попытка распарсивания файла {0} всеми доступными парсерами", file.Name_text));
            var ans = new Dictionary<string, List<TranslationSubstring>>();
            foreach (var pf in ParseFunctions) ans.Add(pf.Key, pf.Value(file));
            var max = ans.First(a => a.Value.Count == ans.Values.Max(v => v.Count));
            _logger.WriteLn(string.Format("Для файла {0} наиболее релевантен '{1}'-парсер (обнаружено записей: {2})", file.Name_text, max.Key, max.Value.Count));
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
            _logger.WriteLn(string.Format("Попытка распарсивания файла {0}", file.Name_text));
            string ext = string.IsNullOrEmpty(extension) ? file.Name_text.Split('.').Last().ToLower() : extension;
            var ts = new List<TranslationSubstring>();
            if (ParseFunctions.ContainsKey(ext)) ts = ParseFunctions[ext](file); else throw new ParserException(ParserException.ParserExceptionTypes.WrongExtension);
            if (ts.Count == 0) throw new ParserException(ParserException.ParserExceptionTypes.NoElements);
            _logger.WriteLn(string.Format("Файл {0} успешно распарсен{1}", file.Name_text, string.IsNullOrEmpty(extension) ? string.Empty : string.Format(" (принудительно применен парсер для расширения '{0}')", ext)));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'po'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsPo(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'po'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?:msgctxt\\s+\"([^\"]*)\"\\s+)?msgid\\s+\"([^\"]*)\"\\s+msgstr\\s+\"([^\"]*)\"";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[3].Value, m.Groups[3].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'po'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'properties'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsProperties(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'properties'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(.*)=(.*)\\s";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'properties'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'json'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsJson(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'json'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*:\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                bool isLatin = !Regex.IsMatch(m.Groups[1].Value, @"\p{IsCyrillic}", RegexOptions.IgnoreCase);
                ts.Add(new TranslationSubstring(m.Groups[isLatin ? 2 : 1].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'json'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'strings'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsStrings(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'strings'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'strings'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'csv'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsCsv(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'csv'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"(?:(?<=\")\"|[^\"])*(?<!\")\"";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[3].Value, m.Groups[3].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'csv'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'xml'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsXml(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'xml'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string simpleRowPattern = "<string\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>([^<]*)</string\\s*>";
            string arrayPattern = "<string-array\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>((?:(?!</string-array).)*)</string-array\\s*>";
            string arrayItemPattern = "<item\\s*>((?:(?!</item).)*)</item\\s*>";
            var matches = Regex.Matches(file.Original_Full_Text, simpleRowPattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            matches = Regex.Matches(file.Original_Full_Text, arrayPattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                string context = m.Groups[1].Value;
                var itemMatches = Regex.Matches(m.Groups[2].Value, arrayItemPattern, RegexOptions.Singleline);
                foreach (Match m2 in itemMatches) ts.Add(new TranslationSubstring(m2.Groups[1].Value, m.Groups[1].Value, file.id, m2.Groups[1].Value, m.Groups[2].Index + m2.Groups[1].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'xml'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'php'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsPhp(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'php'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string arrayElementPattern = "(array\\s*[(]|[[]|=>|(?:[)]|[]])?\\s*,)\\s*((?<!\\\\)'((?:(?<=\\\\)'|[^'])*)(?<!\\\\)'|\\d+)";
            string arrayEndPattern = "(?:[)]|[]])\\s*,\\s*$";
            var matches = Regex.Matches(file.Original_Full_Text, arrayElementPattern);
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
                    if (contextParts.Count > 0 && Regex.IsMatch(matches[i].Groups[1].Value, arrayEndPattern)) contextParts.RemoveAt(contextParts.Count - 1);
                    contextParts.Add(matches[i].Groups[2].Value);
                }
            }
            _logger.WriteLn(string.Format("Парсер 'php'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'resx'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsResx(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'resx'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "<data(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*>(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*<value\\s*>([^<]*)</value\\s*>\\s*(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*</data\\s*>";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'resx'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'string'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsString(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'string'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'string'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'txt'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsTxt(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'txt'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "(.+)\r?\n?";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[1].Value, string.Empty, file.id, m.Groups[1].Value, m.Groups[1].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'txt'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

        /// <summary>
        /// Функция-парсер файлов с расширением 'rc'
        /// </summary>
        /// <param name="file">Файл для распарсивания</param>
        /// <returns>Список объектов <see cref="TranslationSubstring"/></returns>
        private List<TranslationSubstring> ParseAsRc(File file)
        {
            _logger.WriteLn(string.Format("К файлу {0} применяется парсер для файлов с расширением 'rc'", file.Name_text));
            var ts = new List<TranslationSubstring>();
            string pattern = "\\s*(\\d+)\\s*,\\s*\"((?:[^\"]|(?<=\\\\)\")*)\"\\s*";
            var matches = Regex.Matches(file.Original_Full_Text, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.id, m.Groups[2].Value, m.Groups[2].Index));
            }
            _logger.WriteLn(string.Format("Парсер 'rc'-файлов обнаружил в файле {0} записей: {1}", file.Name_text, ts.Count));
            return ts;
        }

    }
}
