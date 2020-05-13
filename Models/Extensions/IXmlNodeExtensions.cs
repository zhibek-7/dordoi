
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Models.DatabaseEntities;

namespace Models.Extensions
{
    public class IXmlNodeExtensions
    {
        public IXmlNodeExtensions(string connectionString)
        {
        }




        public XmlNode AddElement(XmlDocument doc, Element allElements)
        {
            XmlNode productsNode = null;

            if (allElements != null)
            {
                String nameNode = "";
                String nameNodeValue = "";


                //foreach (var elem in allElements)
                //{


                if (allElements != null && allElements.NodeName != null)
                {

                    nameNode = allElements.NodeName.ToString();

                    productsNode = doc.CreateElement(nameNode);

                    // XmlNode = productsNode;
                    if (allElements.NodeOfValue != null)
                    {
                        nameNodeValue = allElements.NodeOfValue.ToString();
                    }

                    if (allElements.AttributeName != null)
                    {
                        foreach (var attr in (IEnumerable<DatabaseEntities.Attribute>)allElements.AttributeName)
                        {
                            string nameAttribute = attr.AttributeName;
                            string nameAttributeValue = attr.AttributeValue;
                            XmlAttribute productAttributeEnd1 = doc.CreateAttribute(nameAttribute);
                            productAttributeEnd1.Value = nameAttributeValue;
                            productsNode.Attributes.Append(productAttributeEnd1);
                        }
                        if (nameNodeValue != "" /*|| nameNodeValue != null*/)
                        {
                            productsNode.AppendChild(doc.CreateTextNode(nameNodeValue));
                            //  productsNode.AppendChild(productsNode);

                        }
                        //  return productsNode;
                    }
                    else
                    {

                        if (nameNodeValue != "" /*|| nameNodeValue != null*/)
                        {
                            productsNode.AppendChild(doc.CreateTextNode(nameNodeValue));
                            //  productsNode.AppendChild(productsNode);

                        }

                        // return productsNode;
                    }

                    // return productsNode;

                }





                // } 
            }
            return productsNode;
        }


        public XmlNode AddNode(XmlDocument doc, String nameNode, String nameNodeValue)
        {
            XmlNode productsNode = doc.CreateElement(nameNode);
            //XmlAttribute productAttributeEnd0 = doc.CreateAttribute("version");
            //productAttributeEnd0.Value = "1.4";
            //productsNode.Attributes.Append(productAttributeEnd0);
            if (nameNodeValue != "" || nameNodeValue != null)
            {
                productsNode.AppendChild(doc.CreateTextNode(nameNodeValue));
            }
            //doc.AppendChild(productsNode);
            return productsNode;
        }


        public XmlNode AddAttributeToNode(XmlDocument doc, XmlNode productNodeEnd, String nameAttribute, String nameAttributeValue/*, String nameNodeValue*/)
        {
            XmlAttribute productAttributeEnd1 = doc.CreateAttribute(nameAttribute);
            productAttributeEnd1.Value = nameAttributeValue;
            productNodeEnd.Attributes.Append(productAttributeEnd1);

            //if (nameNodeValue != "" || nameNodeValue != null)
            //{
            //  productNodeEnd.AppendChild(doc.CreateTextNode(nameNodeValue));
            //}
            ////doc.AppendChild(productsNode);
            //else {
            //    doc.AppendChild(productNodeEnd);
            //}
            return productNodeEnd;
        }
    }
}
