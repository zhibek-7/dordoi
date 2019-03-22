using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    public class Attribute
    {

        //int _id;
        //string _AttributeName;
        //string _AttributeValue;


        public int id { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }


        public Attribute(int id, string AttributeName, string AttributeValue)
        {
            this.id = id;
            this.AttributeName = AttributeName;
            this.AttributeValue = AttributeValue;
        }




    }
}
