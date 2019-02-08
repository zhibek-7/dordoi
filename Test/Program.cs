using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using Models.DatabaseEntities;
using Utilities.Logs;

namespace Test
{
    class Program
    {

        public string WriteLn(Type type, Object obj)
        {
            var stream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(type);


            ser.Serialize(stream, obj);
            stream.Position = 0;
            var sr = new StreamReader(stream);

            return sr.ReadToEnd();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            LogTools lt = new LogTools();
            //lt.WriteLn("--123--");

            Role r = new Role();
            r.Name_text = "роль";
            r.id = 1;
            r.Description = "описание";
            //***********************
            //Program p = new Program();
            //String t = p.WriteLn(typeof(Role), r);
            /*
            var stream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(Role));
            ser.Serialize(stream, r);
            stream.Position = 0;
            var sr = new StreamReader(stream);

            lt.WriteLn(sr.ReadToEnd());
            */


            lt.WriteLn(" 123 ", r.GetType(), r);
            Console.ReadKey();

        }
    }
}
