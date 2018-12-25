using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Models.DatabaseEntities;

namespace Models.Models
{
    public class Parser : IDisposable
    {
        public List<TranslationSubstring> TranslationSubstrings;

        public Parser(File file)
        {
            string ext = file.Name.Split('.').Last().ToLower();
            this.TranslationSubstrings = new List<TranslationSubstring>();
            switch (ext)
            {
                case "po":
                    {
                        this.ParseAsPo(file);
                        break;
                    }
                case "properties":
                    {
                        this.ParseAsProperties(file);
                        break;
                    }
                case "json":
                    {
                        this.ParseAsJson(file);
                        break;
                    }
                case "strings":
                    {
                        this.ParseAsStrings(file);
                        break;
                    }
                case "csv":
                    {
                        this.ParseAsCsv(file);
                        break;
                    }
                case "xml":
                    {
                        ParseAsXml(file);
                        break;
                    }
                case "php":
                    {
                        ParseAsPhp(file);
                        break;
                    }
                case "resx":
                    {
                        ParseAsResx(file);
                        break;
                    }
                case "string":
                    {
                        this.ParseAsString(file);
                        break;
                    }
                case "txt":
                    {
                        this.ParseAsTxt(file);
                        break;
                    }
                case "rc":
                    {
                        this.ParseAsRc(file);
                        break;
                    }
                default:
                    {
                        break;
                        throw new Exception("Файл с данным расширением не поддерживается системой");
                    }
            }
            if (this.TranslationSubstrings.Count == 0) throw new Exception("Элементы для перевода не обнаружены");
        }

        private void ParseAsPo(File file)
        {
            string pattern = "(?:msgctxt\\s+\"([^\"]*)\"\\s+)?msgid\\s+\"([^\"]*)\"\\s+msgstr\\s+\"([^\"]*)\"";
            MatchCollection matches = Regex.Matches(file.OriginalFullText, pattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[3].Value, m.Groups[3].Index));
            }
        }

        private void ParseAsProperties(File file)
        {
            string pattern = "(.*)=(.*)\\s";
            MatchCollection matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
        }

        private void ParseAsJson(File file)
        {
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*:\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                bool isLatin = !Regex.IsMatch(m.Groups[1].Value, @"\p{IsCyrillic}", RegexOptions.IgnoreCase);
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[isLatin ? 2 : 1].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
        }

        private void ParseAsStrings(File file)
        {
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
        }

        private void ParseAsCsv(File file)
        {
            string pattern = "(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"(?:(?<=\")\"|[^\"])*(?<!\")\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[3].Value, m.Groups[3].Index));
            }
        }

        private void ParseAsXml(File file)
        {
            string simpleRowPattern = "<string\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>([^<]*)</string\\s*>";
            string arrayPattern = "<string-array\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>((?:(?!</string-array).)*)</string-array\\s*>";
            string arrayItemPattern = "<item\\s*>((?:(?!</item).)*)</item\\s*>";
            MatchCollection matches = Regex.Matches(file.OriginalFullText, simpleRowPattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            matches = Regex.Matches(file.OriginalFullText, arrayPattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                string context = m.Groups[1].Value;
                MatchCollection itemMatches = Regex.Matches(m.Groups[2].Value, arrayItemPattern, RegexOptions.Singleline);
                foreach (Match m2 in itemMatches) this.TranslationSubstrings.Add(new TranslationSubstring(m2.Groups[1].Value, m.Groups[1].Value, file.ID, m2.Groups[1].Value, m.Groups[2].Index + m2.Groups[1].Index));
            }
        }

        private void ParseAsPhp(File file)
        {
            MatchCollection matches = Regex.Matches(file.OriginalFullText, "(array\\s*[(]|[[]|=>|(?:[)]|[]])?\\s*,)\\s*((?<!\\\\)'((?:(?<=\\\\)'|[^'])*)(?<!\\\\)'|\\d+)", RegexOptions.Singleline);
            List<string> contextParts = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                string context = "array";
                if (Regex.IsMatch(matches[i].Groups[1].Value, "=>"))
                {
                    for (int j = 0; j < contextParts.Count; j++) context += string.Format("[{0}]", contextParts[j]);
                    this.TranslationSubstrings.Add(new TranslationSubstring(matches[i].Groups[3].Value, context, file.ID, matches[i].Groups[3].Value, matches[i].Groups[3].Index));
                    contextParts.RemoveAt(contextParts.Count - 1);
                }
                else
                {
                    if (contextParts.Count > 0 && Regex.IsMatch(matches[i].Groups[1].Value, "(?:[)]|[]])\\s*,\\s*$")) contextParts.RemoveAt(contextParts.Count - 1);
                    contextParts.Add(matches[i].Groups[2].Value);
                }
            }
        }

        private void ParseAsResx(File file)
        {
            string pattern = "<data(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*>(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*<value\\s*>([^<]*)</value\\s*>\\s*(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*</data\\s*>";
            var matches = Regex.Matches(file.OriginalFullText, pattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
        }

        private void ParseAsString(File file)
        {
            string pattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            var matches = Regex.Matches(file.OriginalFullText, pattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
        }

        private void ParseAsTxt(File file)
        {
            string pattern = "(.+)\r?\n?";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[1].Value, string.Empty, file.ID, m.Groups[1].Value, m.Groups[1].Index));
            }
        }

        private void ParseAsRc(File file)
        {
            string pattern = "\\s*(\\d+)\\s*,\\s*\"((?:[^\"]|(?<=\\\\)\")*)\"\\s*";
            var matches = Regex.Matches(file.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(m.Groups[2].Value, m.Groups[1].Value, file.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
        }

        public void Dispose()
        {

        }
    }
}
