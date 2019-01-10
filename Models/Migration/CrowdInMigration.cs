using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace TestParseConsoleApp
{
    class CrowdInMigration
    {
        public CrowdInMigration()
        {

        }

        public void LoadXliff(string filename)
        {
            XDocument d = XDocument.Load(filename);
            XNamespace ns = d.Root.GetDefaultNamespace();
            foreach (XElement file in d.Root.Elements(ns + "file"))
            {
                string original = file.Attribute("original").Value;
                //here we need to check if filename (original variable) exists
                string source_language = file.Attribute("source-language").Value;
                string target_language = file.Attribute("target-language").Value;
                var body = file.Element(ns + "body");
                foreach (XElement el in body.Elements())
                {
                    switch (el.Name.LocalName)
                    {
                        case "trans-unit":
                            {
                                string source = el.Element(ns + "source").Value;
                                string target = el.Element(ns + "target").Value;
                                string note = el.Element(ns + "note").Value;
                                //ok, now it's all about where to write gathered data
                                break;
                            }
                    }
                }
            }
        }

        public void LoadTbx(string filename)
        {
            XDocument d = XDocument.Load(filename);
            XNamespace ns = d.Root.GetNamespaceOfPrefix("xml");
            foreach (XElement text in d.Root.Elements("text"))
            {
                foreach (XElement termEntry in text.Element("body").Elements("termEntry"))
                {
                    foreach (XElement langSet in termEntry.Elements("langSet"))
                    {
                        string lang = langSet.Attribute(ns + "lang").Value;
                        string term = langSet.Element("tig").Element("term").Value;
                        //ok, now it's all about where to write gathered data
                    }
                }
            }
        }
    }
}
