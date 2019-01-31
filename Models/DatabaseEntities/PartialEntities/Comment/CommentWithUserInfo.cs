using System;
using System.Collections.Generic;
using System.Text;
using Models.DatabaseEntities;

namespace Models.DatabaseEntities.PartialEntities.Comment
{
    public class CommentWithUserInfo
    {
        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        public int Comment_Id { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Comment_text { get; set; }

        /// <summary>
        /// Время и дата комментария
        /// </summary>
        public DateTime Date_Time { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int User_Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// Скриншоты прикрепленные к данному комментарию
        /// </summary>
        public IEnumerable<Image> Images { get; set; }
    }
}
