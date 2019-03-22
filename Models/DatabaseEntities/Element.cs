using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    public class Element
    {
        //int _id;
        //string _NodeName;
        //string _NodeOfValue;
        //IEnumerable<Attribute> _AttributeName;

        public int id { get; set; }
        public string NodeName { get; set; }
        public string NodeOfValue { get; set; }
        public IEnumerable<Attribute> AttributeName { get; set; }




        public Element(int id, string NodeName, string NodeOfValue, IEnumerable<Attribute> AttributeName)
        {
            this.id = id;
            this.NodeName = NodeName;
            this.NodeOfValue = NodeOfValue;
            this.AttributeName = AttributeName;
        }


    }
}
