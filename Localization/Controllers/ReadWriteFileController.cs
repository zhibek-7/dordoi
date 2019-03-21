using System;
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


        public ReadWriteFileController()
        {
            string connectionString = Settings.GetStringDB();
            _localizationProjectRepository = new LocalizationProjectRepository(connectionString);
            _localeRepository = new LocaleRepository(connectionString);
            _translationRepository = new TranslationRepository(connectionString);
            _translationSubstringRepository = new TranslationSubstringRepository(connectionString);
            _IXmlNodeExtensions = new IXmlNodeExtensions(connectionString);
        }

        [HttpPost]
        [Route("createTmx")]
        public void TmxCreate(string xmlPath)
        {
            int defaultLang = _localizationProjectRepository.GetByID(0).ID_Source_Locale;
            string defaultLangLocale = _localeRepository.GetByID(defaultLang).code;

            //по id_locale находим translations.id
            IEnumerable<Translation> allTranslations = (IEnumerable<Translation>)_translationRepository.GetAllTranslationsByID_locale(defaultLang);

            //по id_locale находим translations.id
            IEnumerable<TranslationSubstring> substring_to_trans = (IEnumerable<TranslationSubstring>)_translationSubstringRepository.GetStringsInVisibleAndCurrentProjectd(0);

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

            listOfNodesForHeader[0] = new Element(1, "tmx", null, listAttributesForTmx);

            XmlNode listOfNodesForHeader0 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[0]);
            doc.AppendChild(listOfNodesForHeader0);



            listOfNodesForHeader[1] = new Element(2, "header", null, listAttributes);
            XmlNode listOfNodesForHeader1 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[1]);
            listOfNodesForHeader0.AppendChild(listOfNodesForHeader1);



            listOfNodesForHeader[2] = new Element(3, "body", null, null);
            XmlNode listOfNodesForHeader2 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[2]);
            listOfNodesForHeader1.AppendChild(listOfNodesForHeader2);



            foreach (var item in substring_to_trans)
            {
                Attribute[] listAttributesForTu = new Attribute[1];
                listAttributesForTu[0] = new Attribute(1, "tuid", item.id.ToString());
                listOfNodesForHeader[3] = new Element(4, "tu", null, listAttributesForTu);
                XmlNode listOfNodesForHeader3 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[3]);
                listOfNodesForHeader2.AppendChild(listOfNodesForHeader3);





                IEnumerable<Translation> allTranslations1 = (IEnumerable<Translation>)_translationRepository.GetAllTranslationsInStringWithLocale(item.id);
                if (allTranslations1.Count() != 0)
                {

                    Attribute[] listAttributesForTuvP = new Attribute[2];
                    listAttributesForTuvP[0] = new Attribute(1, "xml:lang", defaultLangLocale);
                    listAttributesForTuvP[1] = new Attribute(2, "type", "primary:" + item.id.ToString());
                    listOfNodesForHeader[4] = new Element(5, "tuv", null, listAttributesForTuvP);
                    XmlNode listOfNodesForHeader4 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[4]);
                    listOfNodesForHeader3.AppendChild(listOfNodesForHeader4);


                    listOfNodesForHeader[5] = new Element(6, "seg", item.substring_to_translate, null);
                    XmlNode listOfNodesForHeader5 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[5]);
                    listOfNodesForHeader4.AppendChild(listOfNodesForHeader5);


                    foreach (var item_sub in allTranslations1)
                    {
                        string defaultLangLocaleTranslations = _localeRepository.GetByID(item_sub.ID_Locale).code;

                        Attribute[] listAttributesForTuv = new Attribute[2];
                        listAttributesForTuv[0] = new Attribute(1, "xml:lang", defaultLangLocaleTranslations);
                        listAttributesForTuv[1] = new Attribute(2, "type", "id:" + item_sub.id.ToString());
                        listOfNodesForHeader[6] = new Element(7, "tuv", null, listAttributesForTuv);
                        XmlNode listOfNodesForHeader6 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[6]);
                        listOfNodesForHeader3.AppendChild(listOfNodesForHeader6);


                        listOfNodesForHeader[7] = new Element(8, "seg", item_sub.Translated, null);
                        XmlNode listOfNodesForHeader7 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[7]);
                        listOfNodesForHeader6.AppendChild(listOfNodesForHeader7);

                    }
                }
                else
                {
                    Attribute[] listAttributesForTuvP = new Attribute[2];
                    listAttributesForTuvP[0] = new Attribute(1, "xml:lang", defaultLangLocale);
                    listAttributesForTuvP[1] = new Attribute(2, "type", "primary:" + item.id.ToString());
                    listOfNodesForHeader[4] = new Element(5, "tuv", null, listAttributesForTuvP);
                    XmlNode listOfNodesForHeader4 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[4]);
                    listOfNodesForHeader3.AppendChild(listOfNodesForHeader4);

                    listOfNodesForHeader[5] = new Element(6, "seg", item.substring_to_translate, null);
                    XmlNode listOfNodesForHeader5 = _IXmlNodeExtensions.AddElement(doc, listOfNodesForHeader[5]);
                    listOfNodesForHeader4.AppendChild(listOfNodesForHeader5);
                }
            }
            
            doc.Save("test5.tmx");
        }


    }
}
