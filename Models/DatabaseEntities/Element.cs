using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities
{
    public class Element
    {
        
        public int id { get; set; }
        public string NodeName { get; set; }
        public string NodeOfValue { get; set; }
        public string NodeOfValue1 { get; set; }
        public string NodeOfValue2 { get; set; }
        public IEnumerable<Attribute> AttributeName { get; set; }


        public Element(int id, string NodeName, string NodeOfValue, string NodeOfValue1, string NodeOfValue2, IEnumerable<Attribute> AttributeName)
        {
            this.id = id;
            this.NodeName = NodeName;
            this.NodeOfValue = NodeOfValue;
            this.NodeOfValue1 = NodeOfValue1;
            this.NodeOfValue2 = NodeOfValue2;
            this.AttributeName = AttributeName;
        }


    }
}
