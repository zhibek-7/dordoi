using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DatabaseEntities.DTO
{
    public class Translator : BaseEntity
    {
        //public bool publicProfile { get; set; }, // публичный или скрытый аккаунт
        public string user_name { get; set; }
        public string user_email { get; set; }
        //public string service { get; set; } // услуга (перевод, редактура)
        public Byte[] user_pic { get; set; }
        public int? translation_rating { get; set; } // рейтинг за переводы
        public int? term_rating { get; set; } // рейтинг за сроки
        public int number_of_ratings { get; set; } // количество оценок
        public IEnumerable<string> topics { get; set; } // темы, в которых у переводчика есть компетенция
        public string topics_string { get; set; } // темы, в которых у переводчика есть компетенция
        //public IEnumerable<string> languages { get; set; } // языки, которыми владеет переводчик
        public float words_quantity { get; set; } // переведено слов
        public int? cost { get; set; } // стоимость за слово
        public string currency { get; set; } //валюта стоимости
    }
}
