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
                return NotFound($"File by id \"{ idComment }\" not found");
            }

            var deleteResult = await commentRepository.RemoveAsync(idComment);

            if (!deleteResult)
            {
                return BadRequest($"Failed to remove comment with id \"{ idComment }\" from database");
            }

            return Ok();
        }

    }
}
