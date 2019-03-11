using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : BaseController
    {

        private readonly IUserRepository _userRepository;

        private readonly IImagesRepository _imagesRepository;

        public ImagesController(IUserRepository userRepository, IImagesRepository imagesRepository)
        {
            this._userRepository = userRepository;
            this._imagesRepository = imagesRepository;
        }

        [HttpPost("getByProjectId")]
        [Authorize]
        public async Task<IEnumerable<Image>> GetImagesByProjectIdAsync([FromBody] int projectId)
        {
            var userId = this._userRepository.GetID(this.User.Identity.Name);
            return await this._imagesRepository.GetAllAsync(
                userId: userId,
                projectId: projectId);
        }

    }
}
