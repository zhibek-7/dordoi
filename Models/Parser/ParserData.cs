using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Parser
{
    class ParserData
    {
        public string AllowedExtensionsPattern { get; private set; }
        public string PoPattern { get; private set; }
        public string PropertiesPattern { get; private set; }
        public string JsonPattern { get; private set; }
        public string StringsPattern { get; private set; }
        public string CsvPattern { get; private set; }
        public string XmlSimpleRowPattern { get; private set; }
        public string XmlArrayPattern { get; private set; }
        public string XmlArrayItemPattern { get; private set; }
        public string PhpArrayElementPattern { get; private set; }
        public string PhpArrayEndPattern { get; private set; }
        public string ResxPattern { get; private set; }
        public string StringPattern { get; private set; }
        public string TxtPattern { get; private set; }
        public string RcPattern { get; private set; }

        public ParserData()
        {
            this.AllowedExtensionsPattern = "(po|properties|json|strings|csv|xml|php|resx|string|txt|rc)";
            this.PoPattern = "(?:msgctxt\\s+\"([^\"]*)\"\\s+)?msgid\\s+\"([^\"]*)\"\\s+msgstr\\s+\"([^\"]*)\"";
            this.PropertiesPattern = "(.*)=(.*)\\s";
            this.JsonPattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*:\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            this.StringsPattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            this.CsvPattern = "(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"((?:(?<=\")\"|[^\"])*)(?<!\")\";(?<!\")\"(?:(?<=\")\"|[^\"])*(?<!\")\"";
            this.XmlSimpleRowPattern = "<string\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>([^<]*)</string\\s*>";
            this.XmlArrayPattern = "<string-array\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>((?:(?!</string-array).)*)</string-array\\s*>";
            this.XmlArrayItemPattern = "<item\\s*>((?:(?!</item).)*)</item\\s*>";
            this.PhpArrayElementPattern = "(array\\s*[(]|[[]|=>|(?:[)]|[]])?\\s*,)\\s*((?<!\\\\)'((?:(?<=\\\\)'|[^'])*)(?<!\\\\)'|\\d+)";
            this.PhpArrayEndPattern = "(?:[)]|[]])\\s*,\\s*$";
            this.ResxPattern = "<data(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*[^\\s\\\\/>\"'=]+\\s*=\\s*\"[^\"]*\")*\\s*>(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*<value\\s*>([^<]*)</value\\s*>\\s*(?:\\s*<[^\\s\\\\/>\"'=]+\\s*>[^<]*</[^\\s\\\\/>\"'=]+\\s*>)*\\s*</data\\s*>";
            this.StringPattern = "(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"\\s*=\\s*(?<!\\\\)\"((?:(?<=\\\\)\"|[^\"])*)(?<!\\\\)\"";
            this.TxtPattern = "(.+)\r?\n?";
            this.RcPattern = "\\s*(\\d+)\\s*,\\s*\"((?:[^\"]|(?<=\\\\)\")*)\"\\s*";
        }
    }
}
