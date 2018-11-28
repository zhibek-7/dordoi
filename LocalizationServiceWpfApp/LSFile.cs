using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LocalizationServiceWpfApp
{
    [Table("Files", Schema = "public")]
    public class LSFile
    {
        public int ID { get; set; }
        public int ID_LocalizationProject { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfChange { get; set; }
        public int? StringsCount { get; set; }
        public int? Version { get; set; }
        public int? Priority { get; set; }
        public int? ID_FolderOwner { get; set; }
        [Column("Encoding")]
        public string LSFEncoding { get; set; }
        public bool IsFolder { get; set; }


        public ObservableCollection<LSString> LSStrings;

        private bool _isFileStringsLoaded = false;
        public bool IsFileStringsLoaded
        {
            get { return this._isFileStringsLoaded; }
        }

        public void LoadStrings()
        {
            using (db_Entities context = new db_Entities())
            {
                this.LSStrings = new ObservableCollection<LSString>(context.LSString.Where(lss => lss.ID_FileOwner == this.ID).OrderBy(lss => lss.PositionInFile).ToList());
                _isFileStringsLoaded = true;
            }
        }

        public LSFile() { }

        /// <summary>
        /// Конструктор для создания папки
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="id_LocalizationProject">ID проекта локализации</param>
        /// <param name="name">Имя (короткое) папки</param>
        /// <param name="description">Описание папки</param>
        /// <param name="dateOfChange">Дата последнего изменения папки</param>
        /// <param name="id_FolderOwner">ID папки-владельца папки</param>
        public LSFile(db_Entities context, int id_LocalizationProject, string name, string description, DateTime dateOfChange, int id_FolderOwner)
        {
            //this.ID = context.LSFile.Count() > 0 ? context.LSFile.Select(lsf => lsf.ID).Max() + 1 : 0;
            this.ID_LocalizationProject = id_LocalizationProject;
            this.Name = name;
            this.Description = description;
            this.DateOfChange = dateOfChange;
            this.StringsCount = null;
            this.Version = null;
            this.Priority = null;
            this.ID_FolderOwner = id_FolderOwner;
            this.LSFEncoding = null;
            this.IsFolder = true;
            context.LSFile.Add(this);
            context.SaveChanges();
        }

        /// <summary>
        /// Конструктор для создания файла
        /// </summary>
        /// <param name="context">Контекст подключения к базе данных</param>
        /// <param name="id_LocalizationProject">ID проекта локализации</param>
        /// <param name="fileSafeName">Имя (короткое) файла</param>
        /// <param name="description">Описание файла</param>
        /// <param name="dateOfChange">Дата последнего изменения файла</param>
        /// <param name="version">Версия файла</param>
        /// <param name="priority">Приоритет файла</param>
        /// <param name="id_FolderOwner">ID папки-владельца файла</param>
        /// <param name="fileFullName">Имя (полное) файла</param>
        public LSFile(db_Entities context, string extension, int id_LocalizationProject, string fileSafeName, string description, DateTime dateOfChange, int? version, int? priority, int? id_FolderOwner, string fileFullName)
        {
            //this.ID = context.LSFile.Count() > 0 ? context.LSFile.Select(lsf => lsf.ID).Max() + 1 : 0;
            this.ID_LocalizationProject = id_LocalizationProject;
            this.Name = fileSafeName;
            this.Description = description;
            this.DateOfChange = dateOfChange;
            this.Version = version;
            this.Priority = priority;
            this.ID_FolderOwner = id_FolderOwner;
            this.IsFolder = false;
            string text = File.ReadAllText(fileFullName);
            string[] Lines = ReadAllLinesWithEOLs(text);
            this.StringsCount = Lines.Length;
            using (StreamReader sr = new StreamReader(fileFullName, Encoding.UTF8, true))
            {
                sr.Peek();
                this.LSFEncoding = sr.CurrentEncoding.HeaderName;
            }
            context.LSFile.Add(this);
            context.SaveChanges();
            switch (extension)
            {
                case "po":
                    {
                        this.LSStrings = poFileParse(context, Lines, this.ID, null);
                        break;
                    }
                case "properties":
                    {
                        this.LSStrings = propertiesFileParse(context, Lines, this.ID, null);
                        break;
                    }
                case "json":
                    {
                        this.LSStrings = jsonFileParse(context, Lines, this.ID, null);
                        break;
                    }
                case "strings":
                    {
                        this.LSStrings = stringsFileParse(context, Lines, this.ID, null);
                        break;
                    }
                case "csv":
                    {
                        this.LSStrings = csvFileParse(context, Lines, this.ID, null);
                        break;
                    }
                case "xml":
                    {
                        this.LSStrings = xmlFileParse(context, Lines, this.ID, null);
                        break;
                    }
                case "php":
                    {
                        this.LSStrings = phpFileParse(context, text, this.ID, null);
                        break;
                    }
                case "resx":
                    {
                        this.LSStrings = resxFileParse(context, Lines, this.ID, null);
                        break;
                    }
                case "string":
                    {
                        this.LSStrings = stringFileParse(context, Lines, this.ID, null);
                        break;
                    }

            }
            this._isFileStringsLoaded = true;
            context.SaveChanges();
        }

        private string[] ReadAllLinesWithEOLs(string text)
        {
            List<string> ans = new List<string>();
            int leftBorder = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\r')
                {
                    if (i < text.Length - 1 && text[i + 1] == '\n')
                    {
                        ans.Add(text.Substring(leftBorder, i + 2 - leftBorder));
                        leftBorder = i + 2;
                        i++;
                    }
                    else
                    {
                        ans.Add(text.Substring(leftBorder, i + 1 - leftBorder));
                        leftBorder = i + 1;
                    }
                    continue;
                }
                if (text[i] == '\n')
                {
                    ans.Add(text.Substring(leftBorder, i + 1 - leftBorder));
                    leftBorder = i + 1;
                }
            }
            if (leftBorder < text.Length) ans.Add(text.Substring(leftBorder, text.Length - leftBorder));
            return ans.ToArray();
        }

        private static ObservableCollection<LSString> poFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string contextPattern = "msgctxt(?:\\s)*\"(.*)\"";
            string originalSubstringPattern = "msgid(?:\\s)*\"(.*)\"";
            string translationSubstringPattern = "msgstr(?:\\s)*\"(.*)\"";
            //long stringID = context.LSString.Count() > 0 ? context.LSString.Select(lss => lss.ID).Max() + 1 : 0;
            for (int i = 0; i < lines.Length - 1; i++)
            {
                Match m_originalSubstring = Regex.Match(lines[i], originalSubstringPattern);
                Match m_translationSubstring = Regex.Match(lines[i + 1], translationSubstringPattern);
                if (m_originalSubstring.Success && m_translationSubstring.Success)
                {
                    Match m_context = Regex.Match(i > 0 ? lines[i - 1] : string.Empty, contextPattern);
                    if (m_context.Success) strings.Add(new LSString(context, string.Empty, id_FileOwner, i - 1, lines[i - 1]));
                    strings.Add(new LSString(context, string.Empty, id_FileOwner, i, lines[i]));
                    strings.Add(new LSString(context, m_originalSubstring.Groups[1].Value, string.Empty, m_context.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i + 1, lines[i + 1], m_translationSubstring.Groups[1].Value, m_translationSubstring.Groups[1].Index));
                    i++;
                }
                else
                {
                    Match m_context = Regex.Match(lines[i], contextPattern);
                    if (!m_context.Success) strings.Add(new LSString(context, string.Empty, id_FileOwner, i, lines[i]));
                }
            }
            return strings;
        }

        private static ObservableCollection<LSString> propertiesFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string pattern = "(.*)(?:\\s)*=(?:\\s)*(.*)";
            for (int i = 0; i < lines.Length; i++)
            {
                Match m = Regex.Match(lines[i], pattern);
                if (m.Success)
                {
                    strings.Add(new LSString(context, m.Groups[2].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[2].Value, m.Groups[2].Index));
                }
                else strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
            }
            return strings;
        }

        private static ObservableCollection<LSString> jsonFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string pattern = "\"(.*)\"(?:\\s)*:(?:\\s)*\"(.*)\"(?:,)?";
            for (int i = 0; i < lines.Length; i++)
            {
                Match m = Regex.Match(lines[i], pattern);
                if (m.Success)
                {
                    bool isLatin = !Regex.IsMatch(m.Groups[1].Value, @"\p{IsCyrillic}", RegexOptions.IgnoreCase);
                    strings.Add(new LSString(context, m.Groups[isLatin ? 2 : 1].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[2].Value, m.Groups[2].Index));
                }
                else strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));

            }
            return strings;
        }

        private static ObservableCollection<LSString> stringsFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string pattern = "\"(.*)\"(?:\\s)*=(?:\\s)*\"(.*)\";";
            for (int i = 0; i < lines.Length; i++)
            {
                Match m = Regex.Match(lines[i], pattern);
                if (m.Success)
                {
                    strings.Add(new LSString(context, m.Groups[2].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[2].Value, m.Groups[2].Index));
                }
                else strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
            }
            return strings;
        }

        private static ObservableCollection<LSString> csvFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string pattern = "\"(.*)\";\"(.*)\";\"(.*)\";(?:.)*";
            for (int i = 0; i < lines.Length; i++)
            {
                Match m = Regex.Match(lines[i], pattern);
                if (m.Success)
                {
                    strings.Add(new LSString(context, m.Groups[2].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[3].Value, m.Groups[3].Index));
                }
                else strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
            }
            return strings;
        }

        private static ObservableCollection<LSString> xmlFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string rowPattern = "<string name=\"(.*)\">(.*)</string>";
            string arrayPattern = "<string-array name=\"(.*)\">";
            string endArrayPattern = "(?:\\s)*</string-array>";
            string itemPattern = " <item>(.*)</item>";
            for (int i = 0; i < lines.Length; i++)
            {
                Match m_row = Regex.Match(lines[i], rowPattern);
                if (m_row.Success)
                {
                    strings.Add(new LSString(context, m_row.Groups[2].Value, null, m_row.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m_row.Groups[2].Value, m_row.Groups[2].Index));
                }
                else
                {
                    Match m_array = Regex.Match(lines[i], arrayPattern);
                    if (m_array.Success)
                    {
                        strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
                        i++;
                        while (i < lines.Length)
                        {
                            Match m_item = Regex.Match(lines[i], itemPattern);
                            if (m_item.Success)
                            {
                                strings.Add(new LSString(context, m_item.Groups[1].Value, null, m_array.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m_item.Groups[1].Value, m_item.Groups[1].Index));
                            }
                            else
                            {
                                strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
                                if (Regex.IsMatch(lines[i], endArrayPattern)) break;
                            }
                            i++;
                        }
                    }
                    else strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
                }
            }
            return strings;
        }

        private static ObservableCollection<LSString> phpFileParse(db_Entities context, string text, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            var m = Regex.Matches(text, "'([^']*)'|(\\d+)\\s*=>"); //some rows may contains '/'' which crash next matches
            List<string> LScontextParts = new List<string>();
            int rightBorder = 0;
            int lastRowNumber = 0;
            for (int i = 0; i < m.Count; i++)
            {
                int n = m[i].Index - 1;
                while (n >= 0)
                {
                    if (text[n] == '[' || text[n] == '(')
                    {
                        LScontextParts.Add(m[i].Groups[2].Value == string.Empty ? m[i].Value : m[i].Groups[2].Value);
                        break;
                    }
                    if (text[n] == ',')
                    {
                        LScontextParts.RemoveAt(LScontextParts.Count - 1);
                        if (Regex.IsMatch(text.Substring(0, n).Trim().Last().ToString(), "\\]|\\)")) LScontextParts.RemoveAt(LScontextParts.Count - 1);
                        LScontextParts.Add(m[i].Groups[2].Value == string.Empty ? m[i].Value : m[i].Groups[2].Value);
                        break;
                    }
                    if (text[n] == '>' && n > 0 && text[n - 1] == '=')
                    {
                        n = rightBorder;
                        rightBorder = m[i].Index + m[i].Length;
                        while (text[rightBorder] != '\n') rightBorder++;
                        string[] lines = text.Substring(n, rightBorder - n).Split('\n');
                        int linesNumber = lines.Length;
                        if (m[i].Value.Contains('\n'))
                        {
                            int eolsNumber = m[i].Value.Count(c => c == '\n');
                            for (int j = 0; j < eolsNumber; j++)
                            {
                                lines[lines.Length - 2] += '\n' + lines[lines.Length - 1];
                                Array.Resize(ref lines, lines.Length - 1);
                            }
                        }
                        for (int j = 0; j < lines.Length - 1; j++) strings.Add(new LSString(context, null, id_FileOwner, lastRowNumber + j, lines[j] + '\n'));
                        string LSContext = "array";
                        for (int j = 0; j < LScontextParts.Count; j++) LSContext += "[" + LScontextParts[j] + "]";
                        int posInLine = m[i].Groups[1].Index - strings.Sum(str => str.OriginalStringWithoutEOL.Length) - (strings.Count - 1);
                        strings.Add(new LSString(context, m[i].Groups[1].Value, null, LSContext, defaultTranslationMaxLength, id_FileOwner, lastRowNumber + lines.Length - 1, lines.Last() + '\n', m[i].Groups[1].Value, posInLine));
                        lastRowNumber += linesNumber;
                        rightBorder++;
                        break;
                    }
                    n--;
                }
            }
            string[] lastStrings = text.Substring(rightBorder).Split('\n');
            for (int i = 0; i < lastStrings.Length; i++)
            {
                strings.Add(new LSString(context, null, id_FileOwner, lastRowNumber + i, lastStrings[i] + '\n'));
            }
            return strings;
        }

        private static ObservableCollection<LSString> resxFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string contextPattern = "name[\\s]*=[\\s]*\"(.*)\"";
            string translationPattern = "<value>(.*)</value>";
            for (int i = 0; i < lines.Length; i++)
            {
                Match m_context = Regex.Match(lines[i], contextPattern);
                if (m_context.Success)
                {
                    Match m_translation = Regex.Match(lines[i], translationPattern);
                    if (m_translation.Success)
                    {
                        strings.Add(new LSString(context, m_translation.Groups[1].Value, null, m_context.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m_translation.Groups[1].Value, m_translation.Groups[1].Index));
                    }
                    else
                    {
                        strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
                        if (!Regex.IsMatch(lines[i + 1], contextPattern))
                        {
                            m_translation = Regex.Match(lines[i + 1], translationPattern);
                            if (m_translation.Success)
                            {
                                strings.Add(new LSString(context, m_translation.Groups[1].Value, null, m_context.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i + 1, lines[i + 1], m_translation.Groups[1].Value, m_translation.Groups[1].Index));
                                i++;
                            }
                        }
                    }
                }
                else strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
            }
            return strings;
        }

        private static ObservableCollection<LSString> stringFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        {
            ObservableCollection<LSString> strings = new ObservableCollection<LSString>();
            string pattern = "\"(.*)\"(?:\\s)*=(?:\\s)*\"(.*)\";";
            for (int i = 0; i < lines.Length; i++)
            {
                Match m = Regex.Match(lines[i], pattern);
                if (m.Success)
                {
                    strings.Add(new LSString(context, m.Groups[2].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[2].Value, m.Groups[2].Index));
                }
                else strings.Add(new LSString(context, null, id_FileOwner, i, lines[i]));
            }
            return strings;
        }
    }
}
