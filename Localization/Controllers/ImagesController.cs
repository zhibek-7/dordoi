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


        public class GetImagesByProjectIdAsyncParam
        {
            public int projectId { get; set; }
            public string imageNameFilter { get; set; }
            public int? offset { get; set; }
            public int? limit { get; set; }
        }
        [HttpPost("getByProjectId")]
        [Authorize]
        public async Task<IEnumerable<Image>> GetImagesByProjectIdAsync([FromBody] GetImagesByProjectIdAsyncParam param)
        {
            var userId = this._userRepository.GetID(this.User.Identity.Name).Value;
            this.Response.Headers.Add(
                key: "totalCount",
                value: (await this._imagesRepository
                        .GetFilteredCountAsync(
                            userId: userId,
                            projectId: param.projectId,
                            imageNameFilter: param.imageNameFilter))
                    .ToString());
            return await this._imagesRepository.GetFilteredAsync(
                userId: userId,
                projectId: param.projectId,
                imageNameFilter: param.imageNameFilter,
                limit: param.limit ?? 25,
                offset: param.offset ?? 0
                );
        }

        [HttpPost("update")]
        [Authorize]
        public async Task UpdateImageAsync([FromBody] Image updatedImage)
        {
            await this._imagesRepository.UpdateAsync(updatedImage);
        }

    }
}
