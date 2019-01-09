using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DTO.Files;
using Models.Models;
using Models.Services;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly FilesService _filesService;

        public FilesController(FilesService filesService)
        {
            this._filesService = filesService;
        }

        // GET api/files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node<File>>>> GetAll()
        {
            var files = await this._filesService.GetAll();
            if (files == null)
            {
                return BadRequest("Files not found");
            }

            return Ok(files);
        }

        [HttpGet("byProjectId/{projectId}")]
        public async Task<ActionResult<IEnumerable<Node<File>>>> GetByProjectId(int projectId)
        {
            var files = await this._filesService.GetByProjectIdAsync(projectId: projectId);
            if (files == null)
            {
                return BadRequest("Files not found");
            }

            return Ok(files);
        }

        // GET api/files/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<File>> Get(int id)
        {
            var foundedFile = await this._filesService.GetById(id);
            return Ok(foundedFile);
        }

        // GET api/files/ForProject:ProjectId
        [HttpGet("ForProject:{projectId}")]
        public IEnumerable<File> GetInitialFolders(int projectId)
        {
            var projectFolders = this._filesService.GetInitialFolders(projectId);
            return projectFolders;
        }

        // POST api/files/add/file
        [HttpPost("add/fileByProjectId/{projectId}")]
        public async Task<ActionResult<Node<File>>> AddFile(IFormFile file, [FromForm] int? parentId, int projectId)
        {
            using(var fileContentStream = file.OpenReadStream())
                return await this._filesService.AddFile(
                    fileName: file.FileName,
                    fileContentStream: fileContentStream,
                    parentId: parentId,
                    projectId: projectId);
        }

        // POST api/files/add/folder
        [HttpPost("add/folderByProjectId/{projectId}")]
        public async Task<ActionResult<Node<File>>> AddFolder([FromBody] FolderModel newFolder, int projectId)
        {
            newFolder.ProjectId = projectId;
            return await this._filesService.AddFolder(newFolder);
        }

        // PUT api/files/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateNode(int id, File file)
        {
            await this._filesService.UpdateNode(id: id, file: file);
            return Ok();
        }

        // DELETE api/files/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNode(int id)
        {
            await this._filesService.DeleteNode(id);
            return Ok();
        }

    }

}
