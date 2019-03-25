using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using Models.DatabaseEntities;
using Utilities.Logs;
using External.Migration;
using System.Text.RegularExpressions;

namespace Test
{
    class Program
    {



        static void Main(string[] args)
        {


            // string str = "из-за";// @"TBX (Termbase Exchange format — Обмен терминологическими базами). Это-принятый LISA(Ассоциации индустрии локализации) формат сейчас пересматривается и переиздаётся согласно ISO 30042. Этот стандарт позволяет проводить обмен терминологией,в том числе детальной лексической информацией.";
            //var words = Regex.Split(str.ToLower(), @"\W+");


            // Console.Write(words.Length);

            ////SettingJson sj = new SettingJson();
            //// sj.WriteSettings();


            //using (FileStream fs = new FileStream(@"D:\PJ\Codelink\! Переводчик\_вариант файлов для переводов\_test\taxsee-driver.tbx", FileMode.OpenOrCreate))
            using (FileStream fs = new FileStream(@"D:\PJ\Codelink\! Переводчик\_вариант файлов для переводов\_test\small.tmx", FileMode.OpenOrCreate))
            {

                Import im = new Import();
                im.imp(Import.FileType.TMX, fs);
            }
        }
    }
}
