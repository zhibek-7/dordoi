using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Models.DatabaseEntities;

namespace Models.Parser
{
    public class Parser : IDisposable
    {
        private delegate List<TranslationSubstring> ParseFunction(File file);
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

        public Dictionary<string, List<TranslationSubstring>> UseAllParsers(File file)
        {
            var ans = new Dictionary<string, List<TranslationSubstring>>();
            foreach (var pf in ParseFunctions) ans.Add(pf.Key, pf.Value(file));
            return ans;
        }

        public List<TranslationSubstring> Parse(File file, string extension = "")
        {
            string ext = string.IsNullOrEmpty(extension) ? file.Name.Split('.').Last().ToLower() : extension;
            var ts = new List<TranslationSubstring>();
            if (ParseFunctions.ContainsKey(ext)) ts = ParseFunctions[ext](file); else throw new ParserException(0);
            if (ts.Count == 0) throw new ParserException(1);
            return ts;
        }

        private List<TranslationSubstring> ParseAsPo(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "(?:msgctxt\\s+\"([^\"]*)\"\\s+)?msgid\\s+\"([^\"]*)\"\\s+msgstr\\s+\"([^\"]*)\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[3].Value, m.Groups[3].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsProperties(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "(.*)=(.*)\\s";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsJson(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*:\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                bool isLatin = !Regex.IsMatch(m.Groups[1].Value, @"\p{IsCyrillic}", RegexOptions.IgnoreCase);
                ts.Add(new TranslationSubstring(m.Groups[isLatin ? 2 : 1].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsStrings(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsCsv(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"(?:(?<=\")\"|[^\"])*(?<!\")\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[3].Value, m.Groups[3].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsXml(File file)
        {
            var ts = new List<TranslationSubstring>();
            string simpleRowPattern = "<string\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>([^<]*)</string\\s*>";
            string arrayPattern = "<string-array\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>((?:(?!</string-array).)*)</string-array\\s*>";
            string arrayItemPattern = "<item\\s*>((?:(?!</item).)*)</item\\s*>";
            var matches = Regex.Matches(file.OriginalFullText, simpleRowPattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            matches = Regex.Matches(file.OriginalFullText, arrayPattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                string context = m.Groups[1].Value;
                var itemMatches = Regex.Matches(m.Groups[2].Value, arrayItemPattern, RegexOptions.Singleline);
                foreach (Match m2 in itemMatches) ts.Add(new TranslationSubstring(m2.Groups[1].Value, m.Groups[1].Value, file.ID, m2.Groups[1].Value, m.Groups[2].Index + m2.Groups[1].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsPhp(File file)
        {
            var ts = new List<TranslationSubstring>();
            string arrayElementPattern = "(array\\s*[(]|[[]|=>|(?:[)]|[]])?\\s*,)\\s*((?<!\\\\)'((?:(?<=\\\\)'|[^'])*)(?<!\\\\)'|\\d+)";
            string arrayEndPattern = "(?:[)]|[]])\\s*,\\s*$";
            var matches = Regex.Matches(file.OriginalFullText, arrayElementPattern);
            var contextParts = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                string context = "array";
                if (Regex.IsMatch(matches[i].Groups[1].Value, "=>"))
                {
                    for (int j = 0; j < contextParts.Count; j++) context += string.Format("[{0}]", contextParts[j]);
                    ts.Add(new TranslationSubstring(matches[i].Groups[3].Value, context, file.ID, matches[i].Groups[3].Value, matches[i].Groups[3].Index));
                    contextParts.RemoveAt(contextParts.Count - 1);
                }
                else
                {
                    if (contextParts.Count > 0 && Regex.IsMatch(matches[i].Groups[1].Value, arrayEndPattern)) contextParts.RemoveAt(contextParts.Count - 1);
                    contextParts.Add(matches[i].Groups[2].Value);
                }
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsResx(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "<data(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*>(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*<value\\s*>([^<]*)</value\\s*>\\s*(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*</data\\s*>";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsString(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsTxt(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "(.+)\r?\n?";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[1].Value, string.Empty, file.ID, m.Groups[1].Value, m.Groups[1].Index));
            }
            return ts;
        }

        private List<TranslationSubstring> ParseAsRc(File file)
        {
            var ts = new List<TranslationSubstring>();
            string pattern = "\\s*(\\d+)\\s*,\\s*\"((?:[^\"]|(?<=\\\\)\")*)\"\\s*";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                ts.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            return ts;
        }

        public void Dispose()
        {

        }
    }
}
