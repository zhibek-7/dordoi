using System.Collections.Generic;
using System.Threading.Tasks;
using Localization.Controllers;
using Localization.Hubs.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.DatabaseEntities;

using Models.Models;
using Models.Services;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FilesService _filesService;

        private readonly IHubContext<FilesHub> _hubContext;

        public FilesController(
            FilesService filesService,
            IHubContext<FilesHub> hubContext)
        {
            this._filesService = filesService;
            this._hubContext = hubContext;

            this._filesService.FileParsingFailed += OnFileParsingFailedAsync;
        }

        private async Task OnFileParsingFailedAsync(string signalrClientId, FailedFileParsingModel failedParsingInfoModel)
        {
            await this._hubContext.Clients.Client(signalrClientId).SendAsync(
                FilesHub.ParsingFailedEventName,
                failedParsingInfoModel);
        }

        // GET api/files
        //[Authorize]
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

        //[Authorize]
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
        //[Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<File>> GetAsync(int id)
        {
            var foundedFile = await this._filesService.GetByIdAsync(id);
            return Ok(foundedFile);
        }

        // GET api/files/ForProject:ProjectId
        //[Authorize]
        [HttpGet("ForProject:{projectId}")]
        public IEnumerable<File> GetInitialFolders(int projectId)
        {
            var projectFolders = this._filesService.GetInitialFolders(projectId);
            return projectFolders;
        }

        // POST api/files/add/file
        //[Authorize]
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

        //[Authorize]
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
        //[Authorize]
        [HttpPost("add/folderByProjectId/{projectId}")]
        public async Task<ActionResult<Node<File>>> AddFolderAsync([FromBody] FolderModel newFolder, int projectId)
        {
            newFolder.Project_Id = projectId;
            return await this._filesService.AddFolderAsync(newFolder);
        }

        //[Authorize]
        [HttpPost("upload/folderByProjectId/{projectId}")]
        public async Task UploadFolderWithContentsAsync(
            int projectId,
            IFormFileCollection files,
            [FromForm] int? parentId,
            [FromForm] string signalrClientId)
        {
            await this._filesService.AddFolderWithContentsAsync(
                files: files,
                parentId: parentId,
                projectId: projectId,
                signalrClientId: signalrClientId
                );
        }

        // PUT api/files/update/5
        //[Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateNodeAsync(int id, File file)
        {
            await this._filesService.UpdateNodeAsync(id: id, file: file);
            return Ok();
        }

        // DELETE api/files/delete/5
        //[Authorize]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteNodeAsync([FromBody] File fileToDelete)
        {
            await this._filesService.DeleteNodeAsync(fileToDelete);
            return Ok();
        }

        //[Authorize]
        [HttpGet("{fileId}/changeParentFolder/{newParentId}")]
        public async Task ChangeParentFolderAsync(int fileId, int? newParentId)
        {
            await this._filesService.ChangeParentFolderAsync(
                fileId: fileId,
                newParentId: newParentId
                );
        }

        //[Authorize]
        [HttpGet("{fileId}/locales/list")]
        public async Task<IEnumerable<Locale>> GetTranslationLocalesForFileAsync(int fileId)
        {
            return await this._filesService.GetTranslationLocalesForFileAsync(fileId: fileId);
        }

        //[Authorize]
        [HttpPut("{fileId}/locales")]
        public async Task SetTranslationLocalesForTermAsync(int fileId, [FromBody] IEnumerable<int> localesIds)
        {
            await this._filesService.UpdateTranslationLocalesForTermAsync(
                fileId: fileId,
                localesIds: localesIds);
        }

        //[Authorize]
        [HttpPost("{fileId}/download")]
        public async Task<FileResult> DownloadFileAsync(int fileId, [FromBody] int? localeId)
        {
            var fileStream = await this._filesService.GetFileAsync(fileId, localeId);
            return this.File(
                fileStream: fileStream,
                contentType: "application/octet-stream",
                enableRangeProcessing: true);
        }

        //[Authorize]
        [HttpPost("{fileId}/GetTranslationInfo")]
        public async Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoAsync(int fileId)
        {
            return await this._filesService.GetFileTranslationInfoAsync(fileId: fileId);
        }

    }

}
