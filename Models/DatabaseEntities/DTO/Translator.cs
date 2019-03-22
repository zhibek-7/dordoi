using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    public class Translator
    {
        public Guid? user_Id { get; set; }
        //public bool publicProfile { get; set; }, // публичный или скрытый аккаунт
        public string user_Name { get; set; }
        //public string service { get; set; } // услуга (перевод, редактура)
        public Byte[] user_pic { get; set; }
        public int? translationRating { get; set; } // рейтинг за переводы
        public int? termRating { get; set; } // рейтинг за сроки
        public IEnumerable<string> topics { get; set; } // темы, в которых у переводчика есть компетенция
        //public IEnumerable<string> languages { get; set; } // языки, которыми владеет переводчик
        public int? wordsQuantity { get; set; } // переведено слов
        public int? cost { get; set; } // стоимость за слово
        public string currency { get; set; } //валюта стоимости
    }
}
