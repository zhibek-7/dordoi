using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;

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
        public async Task<ActionResult<IEnumerable<Node<File>>>> GetAllAsync()
        {
            var files = await this._filesService.GetAllAsync();
            if (files == null)
            {
                return BadRequest("Files not found");
            }

            return Ok(files);
        }

        public class GetByProjectIdParams
        {
            public string FileNamesSearch { get; set; }
        }
        [HttpPost("byProjectId/{projectId}")]
        public async Task<ActionResult<IEnumerable<Node<File>>>> GetByProjectIdAsync(int projectId, [FromBody] GetByProjectIdParams param)
        {
            var files = await this._filesService.GetByProjectIdAsync(projectId: projectId, fileNamesSearch: param.FileNamesSearch);
            if (files == null)
            {
                return BadRequest("Files not found");
            }

            return Ok(files);
        }

        // GET api/files/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<File>> GetAsync(int id)
        {
            var foundedFile = await this._filesService.GetByIdAsync(id);
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
        public async Task<ActionResult<Node<File>>> AddFileAsync(IFormFile file, [FromForm] int? parentId, int projectId)
        {
            using (var fileContentStream = file.OpenReadStream())
                return await this._filesService.AddFileAsync(
                    fileName: file.FileName,
                    fileContentStream: fileContentStream,
                    parentId: parentId,
                    projectId: projectId);
        }

        [HttpPost("updateFileVersion/byProjectId/{projectId}")]
        public async Task<ActionResult<Node<File>>> UpdateFileVersionAsync(IFormFile file, [FromForm] int? parentId, int projectId)
        {
            using (var fileContentStream = file.OpenReadStream())
                return await this._filesService.UpdateFileVersionAsync(
                    fileName: file.FileName,
                    fileContentStream: fileContentStream,
                    parentId: parentId,
                    projectId: projectId);
        }

        // POST api/files/add/folder
        [HttpPost("add/folderByProjectId/{projectId}")]
        public async Task<ActionResult<Node<File>>> AddFolderAsync([FromBody] FolderModel newFolder, int projectId)
        {
            return await this._filesService.AddFolderAsync(newFolder);
            newFolder.Project_Id = projectId;
        }

        [HttpPost("upload/folderByProjectId/{projectId}")]
        public async Task UploadFolderWithContentsAsync(IFormFileCollection files, [FromForm] int? parentId, int projectId)
        {
            await this._filesService.AddFolderWithContentsAsync(
                files: files,
                parentId: parentId,
                projectId: projectId
                );
        }

        // PUT api/files/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateNodeAsync(int id, File file)
        {
            await this._filesService.UpdateNodeAsync(id: id, file: file);
            return Ok();
        }

        // DELETE api/files/delete/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteNodeAsync([FromBody] File fileToDelete)
        {
            await this._filesService.DeleteNodeAsync(fileToDelete);
            return Ok();
        }

        [HttpGet("{fileId}/changeParentFolder/{newParentId}")]
        public async Task ChangeParentFolderAsync(int fileId, int? newParentId)
        {
            await this._filesService.ChangeParentFolderAsync(
                fileId: fileId,
                newParentId: newParentId
                );
        }

        [HttpGet("{fileId}/locales/list")]
        public async Task<IEnumerable<Locale>> GetTranslationLocalesForFileAsync(int fileId)
        {
            return await this._filesService.GetTranslationLocalesForFileAsync(fileId: fileId);
        }

        [HttpPut("{fileId}/locales")]
        public async Task SetTranslationLocalesForTermAsync(int fileId, [FromBody] IEnumerable<int> localesIds)
        {
            await this._filesService.UpdateTranslationLocalesForTermAsync(
                fileId: fileId,
                localesIds: localesIds);
        }

        [HttpPost("{fileId}/download")]
        public async Task<FileResult> DownloadFileAsync(int fileId, [FromBody] int? localeId)
        {
            var fileStream = await this._filesService.GetFileAsync(fileId, localeId);
            return this.File(
                fileStream: fileStream,
                contentType: "application/octet-stream",
                enableRangeProcessing: true);
        }

        [HttpPost("{fileId}/GetTranslationInfo")]
        public async Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoAsync(int fileId)
        {
            return await this._filesService.GetFileTranslationInfoAsync(fileId: fileId);
        }

    }

}
