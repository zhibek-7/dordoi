using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Logs;

namespace Models.Parser
{

    /// <summary>
    /// Класс, предназначенный для фиксирования исключений в логике класса <see cref="Parser"/>
    /// </summary>
    public class ParserException : Exception
    {
        // <summary>
        /// Поле, предназначенное для логирования исключений
        /// </summary>
        protected readonly ILogTools _loggerError = new ExceptionLog();

        /// <summary>
        /// Перечисление исключений парсера
        /// </summary>
        public enum ParserExceptionTypes
        {
            WrongExtension,
            NoElements
        };

        /// <summary>
        /// Инициализирует объект класса <see cref="ParserException"/> с указанием типа исключения согласно перечислению <see cref="ParserExceptionTypes"/>
        /// </summary>
        /// <param name="parserExceptionType"></param>
        public ParserException(ParserExceptionTypes parserExceptionType)
        {
            switch((int)parserExceptionType)
            {
                case 0: { this._message = "Файл с данным расширением не поддерживается системой."; break; }
                case 1: { this._message = "Элементы для перевода не обнаружены."; break; }
            }
            _loggerError.WriteLn(string.Format("Исключение в классе {0}: {1}", nameof(Parser),  this._message), this);
        }

        /// <summary>
        /// Описание исключения
        /// </summary>
        private string _message;

        /// <summary>
        /// Свойство, возвращающее описание исключения
        /// </summary>
        public override string Message
        {
            get { return this._message; }
        }
    }
}
