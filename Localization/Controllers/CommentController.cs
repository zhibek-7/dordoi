﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;
using Models.DatabaseEntities.PartialEntities.Comment;
using System.Net.Http;
using System.IO;
using Localization.Controllers;
using Microsoft.AspNetCore.Authorization;
using Utilities;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository commentRepository;
        private readonly TranslationSubstringRepository stringRepository;
        private UserRepository ur;


        public CommentController()
        {
            //this.commentRepository = commentRepository;
            //this.stringRepository = stringRepository;
            var connectionString = Settings.GetStringDB();
            commentRepository = new CommentRepository(connectionString);
            stringRepository = new TranslationSubstringRepository(connectionString);
            ur = new UserRepository(connectionString);
        }


        //TODO  рабочий вариант передачи данны
        //[Serializable]
        //public class Comments2 : BaseEntity
        //{
        //   public Guid? ID_User { get; set; }
        //   public Guid ID_Translation_Substrings { get; set; }
        //    public string Comment_text { get; set; }
        //    public DateTime? DateTime { get; set; }
        //}

        /// <summary>
        /// Добавить комментарий
        /// </summary>
        /// <param name="comment">текст комментария</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> CreateComment([FromBody] Comments comment)//Comments
        {
            //Comments comment2 = null;//comment;
            if (comment == null)
            {
                return BadRequest("Запрос с пустыми параметрами");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Модель не соответсвует");
            }

            comment.ID_User = (Guid)ur.GetID(User.Identity.Name);


            comment.DateTime = DateTime.Now;
            Guid insertedCommentId = (Guid)await commentRepository.AddAsync(comment);
            CommentWithUserInfo commentWithUserInfo = await commentRepository.GetByIDWithUserInfoAsync(insertedCommentId);
            return Ok(commentWithUserInfo);
        }

        /// <summary>
        /// Получает все комментарии которые есть данной фразы
        /// </summary>
        /// <param name="idString">id фразы, комментарии которой необходимы</param>
        /// <returns>Список комментариев</returns>
        [Authorize]
        [HttpPost]
        [Route("InString/{idString}")]
        public async Task<ActionResult<IEnumerable<CommentWithUserInfo>>> GetCommentsInString(Guid idString)
        {
            // Check if string by id exists in database
            var foundedString = await stringRepository.GetByIDAsync(idString);

            if (foundedString == null)
            {
                return NotFound($"File by id \"{ idString }\" not found");
            }

            var commentsInString = await commentRepository.GetAllCommentsInStringByID(idString);
            return Ok(commentsInString);
        }

        //TODO нельзя так делать
        ///// <summary>
        ///// Получает все комментарии
        ///// </summary>
        ///// <returns>Список комментариев</returns>
        //[Authorize]
        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<Comments>>> GetComments()
        //{
        //    IEnumerable<Comments> comments = await commentRepository.GetAllAsync(Guid.Empty);
        //    return Ok(comments);
        //}


        /// <summary>
        /// Удаляет комментарий
        /// </summary>
        /// <param name="idComment">id комментария, который необходимо удалить</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            // Check if string by id exists in database
            var foundedComment = await commentRepository.GetByIDAsync(commentId);

            if (foundedComment == null)
            {
                return NotFound($"Comment by id \"{ commentId }\" not found");
            }

            var deleteResult = await commentRepository.RemoveAsync(commentId);

            if (!deleteResult)
            {
                return BadRequest($"Failed to remove comment with id \"{ commentId }\" from database");
            }

            return Ok();
        }

        /// <summary>
        /// Обновляет комментарий
        /// </summary>
        /// <param name="idComment">id комментария, который нужно обновить</param>
        /// <param name="comment">обновленный комментарий</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("UpdateComment/{idComment}")]
        public async Task<IActionResult> UpdateComment(Guid idComment, [FromBody]  Comments comment)
        {
            // Check if comment by id exists in database
            var foundedComment = await commentRepository.GetByIDAsync(idComment);

            if (foundedComment == null)
            {
                return NotFound($"Comment by id \"{ idComment }\" not found");
            }

            comment.ID_User = (Guid)ur.GetID(User.Identity.Name);

            // Update file in database
            var updateResult = await commentRepository.UpdateAsync(comment);

            if (!updateResult)
            {
                return BadRequest($"Failed to update comment with id \"{idComment}\" in database");
            }

            // Return ok result
            return Ok();
        }

        /// <summary>
        /// Загружает картинку прикрепленную к комментарию
        /// </summary>
        /// <param name="idComment">id комментария к которому приложена картинка</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost()]
        [Route("UploadImageToComment")]
        public async Task<IActionResult> UploadImage()
        {
            var content = Request.Form.Files["Image"];
            var commentId = Request.Form["CommentId"];
            if (content == null)
            {
                return Ok();
            }

            string fileName = content.FileName;
            long fileLength = content.Length;
            //Stream file = content.OpenReadStream;

            if (content != null && fileLength > 0)
            {
                using (var readStream = content.OpenReadStream())
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(content.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)fileLength);

                        Image img = new Image();
                        img.ID_User = (Guid)ur.GetID(User.Identity.Name);
                        img.Name_text = fileName;
                        img.Date_Time_Added = DateTime.Now;
                        img.body = imageData;

                        Guid? insertedCommentId = await commentRepository.UploadImageAsync(img, Guid.Parse(commentId));
                    }
                }


            }
            return Ok();
        }

        ///// <summary>
        ///// Получить изображения комментария
        ///// </summary>
        ///// <param name="commentId">id Комментария</param>
        ///// <returns>Список изображений</returns>
        //public async Task<ActionResult<IEnumerable<Image>>> GetImagesOfComment(int commentId)
        //{
        //    // Check if comment by id exists in database
        //    var foundedComment = await commentRepository.GetByIDAsync(commentId);

        //    if (foundedComment == null)
        //    {
        //        return NotFound($"Comment by id \"{ commentId }\" not found");
        //    }

        //    IEnumerable<Image> images = await commentRepository.GetImagesOfCommentAsync(commentId);

        //    return Ok(images);        
        //}

    }
}
