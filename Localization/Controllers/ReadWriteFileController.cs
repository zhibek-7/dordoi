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

using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Utilities;
using System.Xml;

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

        public ReadWriteFileController()
        {
            string connectionString = Settings.GetStringDB();
            _localizationProjectRepository = new LocalizationProjectRepository(connectionString);
            _localeRepository = new LocaleRepository(connectionString);
            _translationRepository = new TranslationRepository(connectionString);
            _translationSubstringRepository = new TranslationSubstringRepository(connectionString);

            //_localizationProjectsLocalesRepository = new LocalizationProjectsLocalesRepository(connectionString);
            //_localeRepository = new LocaleRepository(connectionString);
            //_userActionRepository = new UserActionRepository(connectionString);
            //ur = new UserRepository(connectionString);
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


            XmlNode productsNode = doc.CreateElement("tmx");
            XmlAttribute productAttributeEnd0 = doc.CreateAttribute("version");
            productAttributeEnd0.Value = "1.4";
            productsNode.Attributes.Append(productAttributeEnd0);
            doc.AppendChild(productsNode);

            XmlNode productNodeEnd = doc.CreateElement("header");
            XmlAttribute productAttributeEnd = doc.CreateAttribute("creationtool");
            productAttributeEnd.Value = "Coderlink";
            productNodeEnd.Attributes.Append(productAttributeEnd);

            XmlAttribute productAttributeEnd1 = doc.CreateAttribute("creationtoolversion");
            productAttributeEnd1.Value = "1.0";
            productNodeEnd.Attributes.Append(productAttributeEnd1);

            XmlAttribute productAttributeEnd2 = doc.CreateAttribute("segtype");
            productAttributeEnd2.Value = "sentence";
            productNodeEnd.Attributes.Append(productAttributeEnd2);

            XmlAttribute productAttributeEnd3 = doc.CreateAttribute("adminlang");
            productAttributeEnd3.Value = "en";
            productNodeEnd.Attributes.Append(productAttributeEnd3);

            XmlAttribute productAttributeEnd4 = doc.CreateAttribute("srclang");
            productAttributeEnd4.Value = defaultLangLocale;//localization_projects. id_source_locale
            productNodeEnd.Attributes.Append(productAttributeEnd4);

            XmlAttribute productAttributeEnd5 = doc.CreateAttribute("o-tmf");
            productAttributeEnd5.Value = "unknown";
            productNodeEnd.Attributes.Append(productAttributeEnd5);

            XmlAttribute productAttributeEnd6 = doc.CreateAttribute("creationid");
            productAttributeEnd6.Value = "Nemo";
            productNodeEnd.Attributes.Append(productAttributeEnd6);

            XmlAttribute productAttributeEnd7 = doc.CreateAttribute("creationdate");
            productAttributeEnd7.Value = DateTime.Now.ToString();
            productNodeEnd.Attributes.Append(productAttributeEnd7);

            productsNode.AppendChild(productNodeEnd);


            XmlNode productNodeBody = doc.CreateElement("body");
            productsNode.AppendChild(productNodeBody);

            ///
            foreach (var item in substring_to_trans)
            {

                XmlNode productNodeTu = doc.CreateElement("tu");
                XmlAttribute productAttributeTu = doc.CreateAttribute("tuid");
                productAttributeTu.Value = item.id.ToString();//translation_substrings.id
                productNodeTu.Attributes.Append(productAttributeTu);
                productNodeBody.AppendChild(productNodeTu);

                //type = "primary:translation_substrings.id"
                IEnumerable<Translation> allTranslations1 = (IEnumerable<Translation>)_translationRepository.GetAllTranslationsInStringWithLocale(item.id);
                if (allTranslations1.Count() != 0)
                {
                    XmlNode productNodeTuvP = doc.CreateElement("tuv");
                    XmlAttribute productAttributeTuvP = doc.CreateAttribute("xml:lang");
                    productAttributeTuvP.Value = defaultLangLocale;
                    productNodeTuvP.Attributes.Append(productAttributeTuvP);

                    XmlAttribute productAttributeTuvTypeP = doc.CreateAttribute("type");
                    productAttributeTuvTypeP.Value = "primary:" + item.id.ToString();
                    productNodeTuvP.Attributes.Append(productAttributeTuvTypeP);

                    productNodeTu.AppendChild(productNodeTuvP);


                    XmlNode productNodeSegP = doc.CreateElement("seg");

                    productNodeSegP.AppendChild(doc.CreateTextNode(item.substring_to_translate));

                    productNodeTuvP.AppendChild(productNodeSegP);

                    foreach (var item_sub in allTranslations1)
                    {
                        string defaultLangLocaleTranslations = _localeRepository.GetByID(item_sub.ID_Locale).code;
                        XmlNode productNodeTuv = doc.CreateElement("tuv");
                        XmlAttribute productAttributeTuv = doc.CreateAttribute("xml:lang");
                        productAttributeTuv.Value = defaultLangLocaleTranslations;
                        productNodeTuv.Attributes.Append(productAttributeTuv);

                        XmlAttribute productAttributeTuvType = doc.CreateAttribute("type");
                        productAttributeTuvType.Value = "id:" + item_sub.id.ToString();
                        productNodeTuv.Attributes.Append(productAttributeTuvType);

                        productNodeTu.AppendChild(productNodeTuv);


                        XmlNode productNodeSeg = doc.CreateElement("seg");

                        productNodeSeg.AppendChild(doc.CreateTextNode(item_sub.Translated));

                        productNodeTuv.AppendChild(productNodeSeg);

                    }

                }
                else
                {


                    XmlNode productNodeTuv = doc.CreateElement("tuv");
                    XmlAttribute productAttributeTuv = doc.CreateAttribute("xml:lang");
                    productAttributeTuv.Value = defaultLangLocale;
                    productNodeTuv.Attributes.Append(productAttributeTuv);

                    XmlAttribute productAttributeTuvType = doc.CreateAttribute("type");
                    productAttributeTuvType.Value = "primary:" + item.id.ToString();
                    productNodeTuv.Attributes.Append(productAttributeTuvType);

                    productNodeTu.AppendChild(productNodeTuv);


                    XmlNode productNodeSeg = doc.CreateElement("seg");

                    productNodeSeg.AppendChild(doc.CreateTextNode(item.substring_to_translate));

                    productNodeTuv.AppendChild(productNodeSeg);


                }


            }


            


            doc.Save("test3.tmx");

        }
    }
}
