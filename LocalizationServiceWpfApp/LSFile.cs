using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LocalizationServiceWpfApp
{
    public class LSFile
    {
        public int ID { get; set; }
        public int ID_LocalizationProject { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateOfChange { get; set; }
        public int? StringsCount { get; set; }
        public int? Version { get; set; }
        public int? Priority { get; set; }
        public int? ID_FolderOwner { get; set; }
        public string LSFEncoding { get; set; }
        public bool IsFolder { get; set; }
        public string OriginalFullText { get; set; }

        public ObservableCollection<TranslationSubstring> TranslationSubstrings;

        public bool IsFileContentLoaded
        {
            get { return this.TranslationSubstrings != null; }
        }

        public void LoadTranslationSubstrings()
        {
            if (!this.IsFileContentLoaded)
            {
                this.TranslationSubstrings = new ObservableCollection<TranslationSubstring>();
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["LocaliztionService"].ConnectionString);
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand("SELECT \"ID\", \"SubstringToTranslate\", \"Description\", \"Context\" , \"TranslationMaxLength\", \"ID_FileOwner\", \"Value\", \"PositionInText\" FROM \"TranslationSubstrings\" WHERE \"ID_FileOwner\" = @ID_FileOwner", conn);
                comm.Parameters.AddWithValue("ID_FileOwner", this.ID);
                NpgsqlDataReader r = comm.ExecuteReader();
                while (r.Read())
                {
                    int ID = r.GetInt32(0);
                    string SubstringToTranslate = r.GetString(1);
                    string Description = r.IsDBNull(2) ? null:  r.GetString(2);
                    string Context = r.GetString(3);
                    int? TranslationMaxLength;
                    if (r.IsDBNull(4)) TranslationMaxLength = null; else TranslationMaxLength = r.GetInt32(4);
                    int ID_FileOwner = r.GetInt32(5);
                    string Value = r.GetString(6);
                    int PositionInText = r.GetInt32(7);
                    this.TranslationSubstrings.Add(new TranslationSubstring(ID, SubstringToTranslate, Description, Context, TranslationMaxLength, ID_FileOwner, Value, PositionInText));
                }
                r.Close();
                conn.Close();
            }
        }

        /// <summary>
        /// Конструктор для выгрузки из БД
        /// </summary>
        public LSFile(int ID, int ID_LocalizationProject, string Name, string Description, DateTime? DateOfChange,int? StringsCount, int? Version, int? Priority,int? ID_FolderOwner, string LSFEncoding, bool IsFolder,string OriginalFullText)
        {
            this.ID = ID;
            this.ID_LocalizationProject = ID_LocalizationProject;
            this.Name = Name;
            this.Description = Description;
            this.DateOfChange = DateOfChange;
            this.StringsCount = StringsCount;
            this.Version = Version;
            this.Priority = Priority;
            this.ID_FolderOwner = ID_FolderOwner;
            this.LSFEncoding = LSFEncoding;
            this.IsFolder = IsFolder;
            this.OriginalFullText = OriginalFullText;
        }

        /// <summary>
        /// Конструктор для распарсивания файла и занесения данных в БД
        /// </summary>
        /// <param name="extension">Расширение файла</param>
        /// <param name="id_LocalizationProject">ID проекта локализации</param>
        /// <param name="fileSafeName">Имя (короткое) файла</param>
        /// <param name="description">Описание файла</param>
        /// <param name="dateOfChange">Дата последнего изменения файла</param>
        /// <param name="version">Версия файла</param>
        /// <param name="priority">Приоритет файла</param>
        /// <param name="id_FolderOwner">ID папки-владельца файла</param>
        /// <param name="fileFullName">Имя (полное) файла</param>
        public LSFile(string extension, int id_LocalizationProject, string fileSafeName, string description, DateTime dateOfChange, int? version, int? priority, int? id_FolderOwner, string fileFullName)
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
            this.OriginalFullText = File.ReadAllText(fileFullName);
            this.StringsCount = File.ReadLines(fileFullName).Count();
            using (StreamReader sr = new StreamReader(fileFullName, Encoding.UTF8, true))
            {
                sr.Peek();
                this.LSFEncoding = sr.CurrentEncoding.HeaderName;
            }
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["LocaliztionService"].ConnectionString);
            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand("INSERT INTO \"Files\" (\"ID_LocalizationProject\", \"Name\" ,\"StringsCount\", \"Encoding\", \"IsFolder\", \"OriginalFullText\") VALUES (@ID_LocalizationProject, @Name , @StringsCount, @Encoding, @IsFolder, @OriginalFullText) RETURNING \"ID\"", conn);
            comm.Parameters.AddWithValue("@ID_LocalizationProject", 0);
            comm.Parameters.AddWithValue("@Name", this.Name);
            comm.Parameters.AddWithValue("@StringsCount", this.StringsCount);
            comm.Parameters.AddWithValue("@Encoding", this.LSFEncoding);
            comm.Parameters.AddWithValue("@IsFolder", this.IsFolder);
            comm.Parameters.AddWithValue("@OriginalFullText", this.OriginalFullText);
            this.ID = (int)comm.ExecuteScalar();
            this.TranslationSubstrings = new ObservableCollection<TranslationSubstring>();
            switch (extension)
            {
                case "po":
                    {
                        this.ParseAsPo(conn);
                        break;
                    }
                case "properties":
                    {
                        this.ParseAsProperties(conn);
                        break;
                    }
                //case "json":
                //    {
                //        this.LSStrings = jsonFileParse(context, Lines, this.ID, null);
                //        break;
                //    }
                //case "strings":
                //    {
                //        this.LSStrings = stringsFileParse(context, Lines, this.ID, null);
                //        break;
                //    }
                //case "csv":
                //    {
                //        this.LSStrings = csvFileParse(context, Lines, this.ID, null);
                //        break;
                //    }
                case "xml":
                    {
                        ParseAsXml(conn);
                        break;
                    }
                case "php":
                    {
                        ParseAsPhp(conn);
                        break;
                    }
                    //case "resx":
                    //    {
                    //        this.LSStrings = resxFileParse(context, Lines, this.ID, null);
                    //        break;
                    //    }
                    //case "string":
                    //    {
                    //        this.LSStrings = stringFileParse(context, Lines, this.ID, null);
                    //        break;
                    //    }

            }
            conn.Close();
        }

        private void ParseAsPo(NpgsqlConnection connection)
        {
            ObservableCollection<TranslationSubstring> strings = new ObservableCollection<TranslationSubstring>();
            string pattern = "(?:msgctxt\\s+\"([^\"]*)\"\\s+)?msgid\\s+\"([^\"]*)\"\\s+msgstr\\s+\"([^\"]*)\"";
            MatchCollection matches = Regex.Matches(this.OriginalFullText, pattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(connection, m.Groups[2].Value, m.Groups[1].Value, this.ID, m.Groups[3].Value, m.Groups[3].Index));
            }
        }

        private void ParseAsProperties(NpgsqlConnection connection)
        {
            ObservableCollection<TranslationSubstring> strings = new ObservableCollection<TranslationSubstring>();
            string pattern = "(.*)=(.*)\\s";
            MatchCollection matches = Regex.Matches(this.OriginalFullText, pattern);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(connection, m.Groups[2].Value, m.Groups[1].Value, this.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
        }

        //private static ObservableCollection<TranslationSubstring> jsonFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        //{
        //    ObservableCollection<TranslationSubstring> strings = new ObservableCollection<TranslationSubstring>();
        //    string pattern = "\"(.*)\"(?:\\s)*:(?:\\s)*\"(.*)\"(?:,)?";
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        Match m = Regex.Match(lines[i], pattern);
        //        if (m.Success)
        //        {
        //            bool isLatin = !Regex.IsMatch(m.Groups[1].Value, @"\p{IsCyrillic}", RegexOptions.IgnoreCase);
        //            strings.Add(new TranslationSubstring(context, m.Groups[isLatin ? 2 : 1].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[2].Value, m.Groups[2].Index));
        //        }
        //        else strings.Add(new TranslationSubstring(context, null, id_FileOwner, i, lines[i]));

        //    }
        //    return strings;
        //}

        //private static ObservableCollection<TranslationSubstring> stringsFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        //{
        //    ObservableCollection<TranslationSubstring> strings = new ObservableCollection<TranslationSubstring>();
        //    string pattern = "\"(.*)\"(?:\\s)*=(?:\\s)*\"(.*)\";";
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        Match m = Regex.Match(lines[i], pattern);
        //        if (m.Success)
        //        {
        //            strings.Add(new TranslationSubstring(context, m.Groups[2].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[2].Value, m.Groups[2].Index));
        //        }
        //        else strings.Add(new TranslationSubstring(context, null, id_FileOwner, i, lines[i]));
        //    }
        //    return strings;
        //}

        //private static ObservableCollection<TranslationSubstring> csvFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        //{
        //    ObservableCollection<TranslationSubstring> strings = new ObservableCollection<TranslationSubstring>();
        //    string pattern = "\"(.*)\";\"(.*)\";\"(.*)\";(?:.)*";
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        Match m = Regex.Match(lines[i], pattern);
        //        if (m.Success)
        //        {
        //            strings.Add(new TranslationSubstring(context, m.Groups[2].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[3].Value, m.Groups[3].Index));
        //        }
        //        else strings.Add(new TranslationSubstring(context, null, id_FileOwner, i, lines[i]));
        //    }
        //    return strings;
        //}

        private void ParseAsXml(NpgsqlConnection connection)
        {
            string simpleRowPattern = "<string\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>([^<]*)</string\\s*>";
            string arrayPattern = "<string-array\\W+(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*\\s*name\\s*=\\s*\"([^\"]*)\"\\s*(?:\\s*\\w+\\s*=\\s*\"[^\"]*\"\\s*)*>((?:(?!</string-array).)*)</string-array\\s*>";
            string arrayItemPattern = "<item\\s*>((?:(?!</item).)*)</item\\s*>";
            MatchCollection matches = Regex.Matches(this.OriginalFullText, simpleRowPattern,RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                this.TranslationSubstrings.Add(new TranslationSubstring(connection, m.Groups[2].Value, m.Groups[1].Value, this.ID, m.Groups[2].Value, m.Groups[2].Index));
            }
            matches = Regex.Matches(this.OriginalFullText, arrayPattern, RegexOptions.Singleline);
            foreach (Match m in matches)
            {
                string context = m.Groups[1].Value;
                MatchCollection itemMatches = Regex.Matches(m.Groups[2].Value, arrayItemPattern,RegexOptions.Singleline);
                foreach (Match m2 in itemMatches) this.TranslationSubstrings.Add(new TranslationSubstring(connection, m2.Groups[1].Value, m.Groups[1].Value, this.ID, m2.Groups[1].Value, m.Groups[2].Index + m2.Groups[1].Index));
            }
        }

        private void ParseAsPhp(NpgsqlConnection connection)
        {
            MatchCollection matches = Regex.Matches(this.OriginalFullText, "(array\\s*[(]|[[]|=>|(?:[)]|[]])?\\s*,)\\s*((?<!\\\\)'((?:(?<=\\\\)'|[^'])*)(?<!\\\\)'|\\d+)", RegexOptions.Singleline);
            List<string> contextParts = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                string context = "array";
                if (Regex.IsMatch(matches[i].Groups[1].Value, "=>"))
                {
                    for (int j = 0; j < contextParts.Count; j++) context += string.Format("[{0}]", contextParts[j]);
                    this.TranslationSubstrings.Add(new TranslationSubstring(connection, matches[i].Groups[3].Value, context, this.ID, matches[i].Groups[3].Value, matches[i].Groups[3].Index));
                    contextParts.RemoveAt(contextParts.Count - 1);
                }
                else
                {
                    if (contextParts.Count > 0 && Regex.IsMatch(matches[i].Groups[1].Value, "(?:[)]|[]])\\s*,\\s*$")) contextParts.RemoveAt(contextParts.Count - 1);
                    contextParts.Add(matches[i].Groups[2].Value);
                }
            }
        }

        //private static ObservableCollection<TranslationSubstring> resxFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        //{
        //    ObservableCollection<TranslationSubstring> strings = new ObservableCollection<TranslationSubstring>();
        //    string contextPattern = "name[\\s]*=[\\s]*\"(.*)\"";
        //    string translationPattern = "<value>(.*)</value>";
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        Match m_context = Regex.Match(lines[i], contextPattern);
        //        if (m_context.Success)
        //        {
        //            Match m_translation = Regex.Match(lines[i], translationPattern);
        //            if (m_translation.Success)
        //            {
        //                strings.Add(new TranslationSubstring(context, m_translation.Groups[1].Value, null, m_context.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m_translation.Groups[1].Value, m_translation.Groups[1].Index));
        //            }
        //            else
        //            {
        //                strings.Add(new TranslationSubstring(context, null, id_FileOwner, i, lines[i]));
        //                if (!Regex.IsMatch(lines[i + 1], contextPattern))
        //                {
        //                    m_translation = Regex.Match(lines[i + 1], translationPattern);
        //                    if (m_translation.Success)
        //                    {
        //                        strings.Add(new TranslationSubstring(context, m_translation.Groups[1].Value, null, m_context.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i + 1, lines[i + 1], m_translation.Groups[1].Value, m_translation.Groups[1].Index));
        //                        i++;
        //                    }
        //                }
        //            }
        //        }
        //        else strings.Add(new TranslationSubstring(context, null, id_FileOwner, i, lines[i]));
        //    }
        //    return strings;
        //}

        //private static ObservableCollection<TranslationSubstring> stringFileParse(db_Entities context, string[] lines, int id_FileOwner, int? defaultTranslationMaxLength)
        //{
        //    ObservableCollection<TranslationSubstring> strings = new ObservableCollection<TranslationSubstring>();
        //    string pattern = "\"(.*)\"(?:\\s)*=(?:\\s)*\"(.*)\";";
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        Match m = Regex.Match(lines[i], pattern);
        //        if (m.Success)
        //        {
        //            strings.Add(new TranslationSubstring(context, m.Groups[2].Value, null, m.Groups[1].Value, defaultTranslationMaxLength, id_FileOwner, i, lines[i], m.Groups[2].Value, m.Groups[2].Index));
        //        }
        //        else strings.Add(new TranslationSubstring(context, null, id_FileOwner, i, lines[i]));
        //    }
        //    return strings;
        //}
    }
}
