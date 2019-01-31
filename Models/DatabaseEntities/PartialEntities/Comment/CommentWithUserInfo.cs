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
        public int CommentId { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Время и дата комментария
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Скриншоты прикрепленные к данному комментарию
        /// </summary>
        public IEnumerable<Image> Images { get; set; }
    }
}
