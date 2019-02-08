using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;

namespace Models.DatabaseEntities.PartialEntities.Comment
{
    [Serializable]
    public class CommentWithUserInfo
    {
        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        public int comment_id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string comment_text { get; set; }

        /// <summary>
        /// Время и дата комментария
        /// </summary>
        public DateTime datetime { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int user_Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// Скриншоты прикрепленные к данному комментарию
        /// </summary>
        public IEnumerable<Image> images { get; set; }
    }
}
