﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;
using Models.DatabaseEntities.Comment;
using System.Net.Http;
using System.IO;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository commentRepository;
        private readonly TranslationSubstringRepository stringRepository;

        public CommentController()
        {
            //this.commentRepository = commentRepository;
            //this.stringRepository = stringRepository;
            commentRepository = new CommentRepository(Settings.GetStringDB());
            stringRepository = new TranslationSubstringRepository(Settings.GetStringDB());
        }

        /// <summary>
        /// Добавить комментарий
        /// </summary>
        /// <param name="comment">текст комментария</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> CreateComment([FromBody] Comments comment)
        {
            if (comment == null)
            {
                return BadRequest("Запрос с пустыми параметрами");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Модель не соответсвует");
            }

            comment.DateTime = DateTime.Now;
            int insertedCommentId = await commentRepository.AddAsync(comment);
            CommentWithUserInfo commentWithUserInfo = await commentRepository.GetByIDWithUserInfoAsync(insertedCommentId);
            return Ok(commentWithUserInfo);
        }

        /// <summary>
        /// Получает все комментарии которые есть данной фразы
        /// </summary>
        /// <param name="idString">id фразы, комментарии которой необходимы</param>
        /// <returns>Список комментариев</returns>
        [HttpGet]
        [Route("InString/{idString}")]
        public async Task<ActionResult<IEnumerable<CommentWithUserInfo>>> GetCommentsInString(int idString)
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

        /// <summary>
        /// Получает все комментарии
        /// </summary>
        /// <returns>Список комментариев</returns>
        public async Task<ActionResult<IEnumerable<Comments>>> GetComments()
        {
            IEnumerable<Comments> comments = await commentRepository.GetAllAsync();
            return Ok(comments);
        }


        /// <summary>
        /// Удаляет комментарий
        /// </summary>
        /// <param name="idComment">id комментария, который необходимо удалить</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteComment/{idComment}")]
        public async Task<IActionResult> DeleteComment(int idComment)
        {
            // Check if string by id exists in database
            var foundedComment = await commentRepository.GetByIDAsync(idComment);

            if (foundedComment == null)
            {
                return NotFound($"Comment by id \"{ idComment }\" not found");
            }

            var deleteResult = await commentRepository.RemoveAsync(idComment);

            if (!deleteResult)
            {
                return BadRequest($"Failed to remove comment with id \"{ idComment }\" from database");
            }

            return Ok();
        }

        /// <summary>
        /// Обновляет комментарий
        /// </summary>
        /// <param name="idComment">id комментария, который нужно обновить</param>
        /// <param name="comment">обновленный комментарий</param>
        /// <returns></returns>
        [HttpPut("UpdateComment/{idComment}")]
        public async Task<IActionResult> UpdateComment(int idComment, Comments comment)
        {
            // Check if comment by id exists in database
            var foundedComment = await commentRepository.GetByIDAsync(idComment);

            if (foundedComment == null)
            {
                return NotFound($"Comment by id \"{ idComment }\" not found");
            }

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
        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage()
        {

            var content = Request.Form.Files["Image"];
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
                        img.ID_User = 301;
                        img.Name = fileName;
                        img.DateTimeAdded = DateTime.Now;
                        img.Data = imageData;

                        int insertedCommentId = await commentRepository.AddFileAsync(img);
                    }
                }


            }
            return Ok();
        }

    }
}
