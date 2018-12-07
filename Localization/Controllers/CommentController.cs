using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Reposity.PostgreSqlRepository;
using Models.DatabaseEntities;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [EnableCors("SiteCorsPolicy")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository commentRepository;

        public CommentController()
        {
            commentRepository = new CommentRepository();
        }

        public List<Comments> GetComments()
        {
            List<Comments> comments = commentRepository.GetAll().ToList();
            return comments;
        }
    }
}