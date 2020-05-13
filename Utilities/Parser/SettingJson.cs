using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Web;


namespace Utilities.Parser
{
    /// <summary>
    /// Парсинг файла настроек
    /// </summary>
    public class SettingJson
    {
        string appsettingsJson = @"appsettings.json";

        public string WriteJson()
        {
            // чтение из файла
            using (FileStream fstream = File.OpenRead(appsettingsJson))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                //Console.WriteLine("Текст из файла: {0}", textFromFile);

                return textFromFile;
            }
        }

        /// <summary>
        /// Парсим файл json в словарь
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        Dictionary<string, string> ParseJson(string res)
        {
            var lines = res.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var ht = new Dictionary<string, string>(20);
            var st = new Stack<string>(20);

            for (int i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];
                var pair = line.Split(":".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);

                if (pair.Length == 2)
                {
                    var key = ClearString(pair[0]);
                    var val = ClearString(pair[1]);

                    if (val == "{")
                    {
                        st.Push(key);
                    }
                    else
                    {
                        if (st.Count > 0)
                        {
                            key = string.Join("_", st) + "_" + key;
                        }

                        if (ht.ContainsKey(key))
                        {
                            ht[key] += "&" + val;
                        }
                        else
                        {
                            ht.Add(key, val);
                        }
                    }
                }
                else if (line.IndexOf('}') != -1 && st.Count > 0)
                {
                    st.Pop();
                }
            }

            return ht;
        }

        string ClearString(string str)
        {
            str = str.Trim();

            var ind0 = str.IndexOf('"');
            var ind1 = str.LastIndexOf('"');


            if (ind0 != -1 && ind1 != -1)
            {
                str = str.Substring(ind0 + 1, ind1 - ind0 - 1);
            }
            else if (str[str.Length - 1] == ',')
            {
                str = str.Substring(0, str.Length - 1);
            }

            str = HttpUtility.UrlDecode(str);

            return str;
        }


        public Dictionary<string, string> WriteSettings(string fileName)
        {
            appsettingsJson = fileName;
            string body = WriteJson();
            Dictionary<string, string> settings = ParseJson(body);
            return settings;
        }

        public Dictionary<string, string> WriteSettings()
        {
            return WriteSettings(@"appsettings.json");
        }
    }
}
