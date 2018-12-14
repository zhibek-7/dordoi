using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;
using Models.Comments;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository commentRepository;
        private readonly StringRepository stringRepository;

        public CommentController()
        {
            commentRepository = new CommentRepository();
            stringRepository = new StringRepository();
        }

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
        public async Task<ActionResult<IEnumerable<Comments>>>  GetComments()
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

    }
}
