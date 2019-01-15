using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Parser
{
    public class ParserException : Exception
    {

        public readonly int ParserExceptionType;
        private Dictionary<int, string> ParserExceptionTypes = new Dictionary<int, string>
        {
            {0, "файл с данным расширением не поддерживается системой"},
            {1, "элементы для перевода не обнаружены" }
        };

        public ParserException(int parserExceptionType)
        {
            this.ParserExceptionType = parserExceptionType;
        }

        public override string Message
        {
            get { return this.ParserExceptionTypes[this.ParserExceptionType]; }
        }
    }
}
