﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DAL.Reposity.PostgreSqlRepository;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Models.Extensions;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Utilities;
using System.Xml;
using Attribute = Models.DatabaseEntities.Attribute;
using Models.DatabaseEntities.PartialEntities.Translation;
using System.IO;
using CsvHelper;
using System.Text;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class ReadWriteFileController : ControllerBase
    {
        private readonly LocalizationProjectRepository _localizationProjectRepository;
        private readonly LocaleRepository _localeRepository;
        private readonly TranslationRepository _translationRepository;
        private readonly TranslationSubstringRepository _translationSubstringRepository;
        private readonly IXmlNodeExtensions _IXmlNodeExtensions;
    
        private readonly GlossaryRepository _glossaryRepository;
        private readonly PartOfSpeechRepository _partOfSpeechRepository;

        public ReadWriteFileController()
        {
            string connectionString = Settings.GetStringDB();
            _localizationProjectRepository = new LocalizationProjectRepository(connectionString);
            _localeRepository = new LocaleRepository(connectionString);
            _translationRepository = new TranslationRepository(connectionString);
            _translationSubstringRepository = new TranslationSubstringRepository(connectionString);
            _IXmlNodeExtensions = new IXmlNodeExtensions(connectionString);
            _glossaryRepository = new GlossaryRepository(connectionString);
            _partOfSpeechRepository = new PartOfSpeechRepository(connectionString);
        }

        [HttpPost]
        [Route("createTmx")]
        public void TmxCreate(string xmlPath)
        {

            //TODO потом переделать
            Guid defaultLang = Guid.Empty;//_localizationProjectRepository.GetByID(Guid.NewGuid()).ID_Source_Locale;
            string defaultLangLocale = _localeRepository.GetByID(defaultLang).code;

            //по id_locale находим translations.id
            IEnumerable<Translation> allTranslations = (IEnumerable<Translation>)_translationRepository.GetAllTranslationsByID_locale(defaultLang);

            //по id_locale находим translations.id
            IEnumerable<TranslationSubstring> substring_to_trans = (IEnumerable<TranslationSubstring>)_translationSubstringRepository.GetStringsInVisibleAndCurrentProjectd(Guid.Empty);

            XmlDocument doc = new XmlDocument();

            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);




            Attribute[] listAttributesForTmx = new Attribute[1];
            listAttributesForTmx[0] = new Attribute(1, "version", "1.4");




            Attribute[] listAttributesForHeader = new Attribute[8];
            listAttributesForHeader[0] = new Attribute(1, "creationtool", "Coderlink");
            listAttributesForHeader[1] = new Attribute(2, "creationtoolversion", "1.0");
            listAttributesForHeader[2] = new Attribute(3, "segtype", "sentence");
            listAttributesForHeader[3] = new Attribute(4, "adminlang", "en");
            listAttributesForHeader[4] = new Attribute(5, "srclang", defaultLangLocale);
            listAttributesForHeader[5] = new Attribute(6, "o-tmf", "unknown");
            listAttributesForHeader[6] = new Attribute(7, "creationid", "Nemo");
            listAttributesForHeader[7] = new Attribute(8, "creationdate", DateTime.Now.ToString());

            IEnumerable<Attribute> listAttributes = new List<Attribute>();
            listAttributes = listAttributesForHeader;
            Element[] listOfNodesForHeader = new Element[8];

            listOfNodesForHeader[0] = new Element(1, "tmx", null, null, null, listAttributesForTmx);

            XmlNode listOfNodesForHeader0 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[0]);
            doc.AppendChild(listOfNodesForHeader0);



            listOfNodesForHeader[1] = new Element(2, "header", null, null, null, listAttributes);
            XmlNode listOfNodesForHeader1 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[1]);
            listOfNodesForHeader0.AppendChild(listOfNodesForHeader1);



            listOfNodesForHeader[2] = new Element(3, "body", null, null, null, null);
            XmlNode listOfNodesForHeader2 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[2]);
            listOfNodesForHeader1.AppendChild(listOfNodesForHeader2);



            foreach (var item in substring_to_trans)
            {
                Attribute[] listAttributesForTu = new Attribute[1];
                listAttributesForTu[0] = new Attribute(1, "tuid", item.id.ToString());
                listOfNodesForHeader[3] = new Element(4, "tu", null, null, null, listAttributesForTu);
                XmlNode listOfNodesForHeader3 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[3]);
                listOfNodesForHeader2.AppendChild(listOfNodesForHeader3);





                IEnumerable<Translation> allTranslations1 = (IEnumerable<Translation>)_translationRepository.GetAllTranslationsInStringWithLocale(item.id);
                if (allTranslations1.Count() != 0)
                {

                    Attribute[] listAttributesForTuvP = new Attribute[2];
                    listAttributesForTuvP[0] = new Attribute(1, "xml:lang", defaultLangLocale);
                    listAttributesForTuvP[1] = new Attribute(2, "type", "primary:" + item.id.ToString());
                    listOfNodesForHeader[4] = new Element(5, "tuv", null, null, null, listAttributesForTuvP);
                    XmlNode listOfNodesForHeader4 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[4]);
                    listOfNodesForHeader3.AppendChild(listOfNodesForHeader4);


                    listOfNodesForHeader[5] = new Element(6, "seg", item.substring_to_translate, null, null, null);
                    XmlNode listOfNodesForHeader5 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[5]);
                    listOfNodesForHeader4.AppendChild(listOfNodesForHeader5);


                    foreach (var item_sub in allTranslations1)
                    {
                        string defaultLangLocaleTranslations = _localeRepository.GetByID(item_sub.ID_Locale).code;

                        Attribute[] listAttributesForTuv = new Attribute[2];
                        listAttributesForTuv[0] = new Attribute(1, "xml:lang", defaultLangLocaleTranslations);
                        listAttributesForTuv[1] = new Attribute(2, "type", "id:" + item_sub.id.ToString());
                        listOfNodesForHeader[6] = new Element(7, "tuv", null, null, null, listAttributesForTuv);
                        XmlNode listOfNodesForHeader6 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[6]);
                        listOfNodesForHeader3.AppendChild(listOfNodesForHeader6);


                        listOfNodesForHeader[7] = new Element(8, "seg", item_sub.Translated, null, null, null);
                        XmlNode listOfNodesForHeader7 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[7]);
                        listOfNodesForHeader6.AppendChild(listOfNodesForHeader7);

                    }
                }
                else
                {
                    Attribute[] listAttributesForTuvP = new Attribute[2];
                    listAttributesForTuvP[0] = new Attribute(1, "xml:lang", defaultLangLocale);
                    listAttributesForTuvP[1] = new Attribute(2, "type", "primary:" + item.id.ToString());
                    listOfNodesForHeader[4] = new Element(5, "tuv", null, null, null, listAttributesForTuvP);
                    XmlNode listOfNodesForHeader4 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[4]);
                    listOfNodesForHeader3.AppendChild(listOfNodesForHeader4);

                    listOfNodesForHeader[5] = new Element(6, "seg", item.substring_to_translate, null, null, null);
                    XmlNode listOfNodesForHeader5 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[5]);
                    listOfNodesForHeader4.AppendChild(listOfNodesForHeader5);
                }
            }





            doc.Save("test3.tmx");

        }

        [HttpPost]
        [Route("createCsv")]
        public void CsvCreate(string xmlPath)
        {
            Guid guid_pj = new Guid("bf18c5ed-706d-4efd-a656-abfe9b320cf7");// Guid.NewGuid();

            Guid defaultLang = _localizationProjectRepository.GetByID(guid_pj).ID_Source_Locale;

            //язык 
            string defaultLangLocale = _localeRepository.GetByID(defaultLang).code;


            string defaultDescriptionLocale = _localeRepository.GetByID(defaultLang).description;


            var guidGlossary = _localizationProjectRepository.GetByIdProjectAsync(guid_pj);

            //IEnumerable<Translation> tr_id_locale = _translationRepository.GetAllTranslationsByID_locale(defaultLang);

            IEnumerable<TranslationSubstringPartOfSpeech> translationSubstrings = _translationSubstringRepository.GetByGlossaryId(guidGlossary.id_glossary);


           // IEnumerable<PartOfSpeech> partOfSpeeches = _partOfSpeechRepository.GetPartOfSpeechByGlossaryId(guidGlossary.id_glossary);


            Element[] listOfNodesForHeader = new Element[translationSubstrings.Count() + 2];

            listOfNodesForHeader[0] = new Element(1, "Term" + "[" + defaultLangLocale + "]", "Description" + "[" + defaultLangLocale + "]", "Part of Speech" + "[" + defaultLangLocale + "]", null, null);

            int i = 1;

            var selectedLocales = translationSubstrings.GroupBy(x => x.l_code).Select(x => x.First());

            foreach (var item in translationSubstrings.GroupBy(x => x.l_code).Select(x => x.First()))
            {
                listOfNodesForHeader[i] = new Element(i + 1, "Term" + "[" + item.l_code + "]", "Description" + "[" + item.l_code + "]", "Part of Speech" + "[" + item.l_code + "]", null, null);
                i++;
            }

            Element[] listOfNodesForHeader1 = new Element[translationSubstrings.Count() + 1];
            int j = 0;
            foreach (var item in translationSubstrings.GroupBy(x => x.id_string).Select(x => x.First()))
            {

                List<Attribute> listTranslated = new List<Attribute>();
                int f = 0;
                foreach (var it in translationSubstrings.GroupBy(x => x.l_code).Select(x => x.First()))
                {
                    f = f++;
                    var lCode = translationSubstrings.Where(x => x.id_string == item.id_string).Select(x => x.l_code).Contains(it.l_code);
                    if (lCode)
                    {
                        var lCode1 = translationSubstrings.Where(x => x.id_string == item.id_string && x.l_code == it.l_code).FirstOrDefault();
                        listTranslated.Add(

                         new Attribute(f, lCode1.l_code, lCode1.t_translated)
                         {
                             id = f,
                             AttributeName = "Term" + "[" + lCode1.l_code + "]",
                             AttributeValue = lCode1.t_translated

                         });
                    }
                    else
                    {
                      listTranslated.Add(
                         new Attribute(f, null, null)
                         {
                             id = f,
                             AttributeName = null,
                             AttributeValue = null
                         });

                    }

                }
                j++;
                listOfNodesForHeader1[j] = new Element(j + 1, item.ts_substring_to_translate, item.ts_description, item.ps_name_text, item.t_translated, listTranslated);
            }
           var utf8_Bom = new System.Text.UTF8Encoding(true); // true to use bom.
            using (var sw = new StreamWriter("testCsv.csv"))
            {

                var writer = new CsvWriter(sw);
                //собираем header
                int k = 0;
                foreach (Element item in listOfNodesForHeader)
                {
                    k++;
                    if (item != null)
                    {
                        writer.WriteField(item.NodeName);
                        writer.WriteField( item.NodeOfValue);
                        writer.WriteField(item.NodeOfValue1);
                    }
                }

                writer.NextRecord();

                //собираем значения

                foreach (Element item1 in listOfNodesForHeader1)
                {
                   if (item1 != null)
                    {
                        i = 0;
                        writer.WriteField(item1.NodeName);
                        writer.WriteField(item1.NodeOfValue);
                        writer.WriteField(item1.NodeOfValue1);
                        if (item1 != null)
                        {
                            j = 0;
                            foreach (var item2 in item1.AttributeName)
                            {
                                var select = item1.AttributeName.FirstOrDefault();
                                if (j == 0)
                                {
                                   writer.WriteField(item2.AttributeValue);
                                }
                                else
                                {
                                    for (int l = 1; l < 3; l++)
                                    {
                                        writer.WriteField(null);
                                    }
                                    writer.WriteField(item2.AttributeValue);
                                }
                                j++;
                            }
                            writer.NextRecord();
                        }

                    }

                }

                Encoding.UTF8.GetBytes(sw.ToString());
                writer.Flush();
                sw.Close();
                sw.Dispose();


            }
        }


    }
}
