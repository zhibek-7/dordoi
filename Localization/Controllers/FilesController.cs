using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Localization.Controllers;
using Localization.Hubs.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.DatabaseEntities;

using Models.Models;
using Models.Services;
using Utilities;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FilesService _filesService;

        private readonly IHubContext<FilesHub> _hubContext;
        private UserRepository ur;

        public FilesController(
            FilesService filesService,
            IHubContext<FilesHub> hubContext)
        {
            this._filesService = filesService;
            this._hubContext = hubContext;

            this._filesService.FileParsingFailed += OnFileParsingFailedAsync;
            ur = new UserRepository(Settings.GetStringDB());
        }

        private async Task OnFileParsingFailedAsync(string signalrClientId, FailedFileParsingModel failedParsingInfoModel)
        {

            await this._hubContext.Clients.Client(signalrClientId).SendAsync(
                FilesHub.ParsingFailedEventName,
                failedParsingInfoModel);

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Node<File>>>> GetAllAsync([FromForm] Guid? project)
        {
            try
            {
                var identityName = User.Identity.Name;
                Guid? userId = (Guid)ur.GetID(identityName);

                var files = await this._filesService.GetAllAsync(userId, project);
                if (files == null)
                {
                    return BadRequest("Файлы не найдены");
                }

                return Ok(files);
            }
            catch (Exception exc)
            {
                return BadRequest("ERROR:" + exc.Message);
            }
        }

        public class GetByProjectIdParams
        {
            public string FileNamesSearch { get; set; }
        }

        [HttpPost("byProjectId/{projectId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Node<File>>>> GetByProjectIdAsync(Guid projectId, [FromBody] GetByProjectIdParams param)
        {
            try
            {
                var files = await this._filesService.GetByProjectIdAsync(projectId: projectId, fileNamesSearch: param.FileNamesSearch);
                if (files == null)
                {
                    return BadRequest("Файлы не найдены");
                }

                return Ok(files);
            }
            catch (Exception exc)
            {
                return BadRequest("ERROR:" + exc.Message);
            }
        }


        [HttpPost]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<File>> GetAsync(Guid id)
        {

            try
            {
                var foundedFile = await this._filesService.GetByIdAsync(id);
                return Ok(foundedFile);
            }
            catch (Exception exc)
            {
                return BadRequest("ERROR:" + exc.Message);
            }
        }

        [HttpPost]
        [Route("ForProject:{projectId}")]
        [Authorize]
        public IEnumerable<File> GetInitialFolders(Guid projectId)
        {

            var projectFolders = this._filesService.GetInitialFolders(projectId);
            return projectFolders;

        }

        [HttpPost("add/fileByProjectId/{projectId}")]
        [Authorize]
        public async Task<ActionResult<Node<File>>> AddFileAsync(IFormFile file, [FromForm] Guid? parentId, Guid projectId)
        {
            try
            {
                using (var fileContentStream = file.OpenReadStream())
                {
                    var t = await this._filesService.AddFileAsync(
                        fileName: file.FileName,
                        fileContentStream: fileContentStream,
                        parentId: parentId,
                        projectId: projectId);
                    return t;
                }

            }
            catch (Exception exc)
            {

                return Conflict(exc);
                //           return BadRequest("ERROR:" + exc.Message);
            }
        }

        [HttpPost("updateFileVersion/byProjectId/{projectId}")]
        [Authorize]
        public async Task<ActionResult<Node<File>>> UpdateFileVersionAsync(IFormFile file, [FromForm] Guid? parentId, Guid projectId)
        {
            try
            {
                using (var fileContentStream = file.OpenReadStream())
                    return await this._filesService.UpdateFileVersionAsync(
                        fileName: file.FileName,
                        fileContentStream: fileContentStream,
                        parentId: parentId,
                        projectId: projectId);
            }
            catch (Exception exc)
            {
                return BadRequest("ERROR:" + exc.Message);
            }
        }

        [HttpPost("add/folderByProjectId/{projectId}")]
        [Authorize]
        public async Task<ActionResult<Node<File>>> AddFolderAsync([FromBody] FolderModel newFolder, Guid projectId)
        {
            try
            {
                //newFolder.projectId = projectId;
                return await this._filesService.AddFolderAsync(newFolder, projectId);
            }
            catch (Exception exc)
            {
                return BadRequest("ERROR:" + exc.Message);
            }
        }

        [HttpPost("upload/folderByProjectId/{projectId}")]
        [Authorize]
        public async Task UploadFolderWithContentsAsync(
            Guid projectId,
            IFormFileCollection files,
            [FromForm] Guid? parentId,
            [FromForm] string signalrClientId)
        {
            await this._filesService.AddFolderWithContentsAsync(
                files: files,
                parentId: parentId,
                projectId: projectId,
                signalrClientId: signalrClientId
                );
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNodeAsync(Guid id, File file)
        {
            try
            {
                await this._filesService.UpdateNodeAsync(id: id, file: file);
                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest("ERROR:" + exc.Message);
            }
        }

        [HttpPost("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteNodeAsync([FromBody] File fileToDelete)
        {
            try
            {
                await this._filesService.DeleteNodeAsync(fileToDelete);
                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest("ERROR:" + exc.Message);
            }
        }

        [HttpPost]
        [Route("{fileId}/changeParentFolder/{newParentId}")]
        [Authorize]
        public async Task ChangeParentFolderAsync(Guid fileId, Guid? newParentId)
        {
            await this._filesService.ChangeParentFolderAsync(
                fileId: fileId,
                newParentId: newParentId
                );
        }

        [HttpPost]
        [Route("{fileId}/locales/list")]
        [Authorize]
        public async Task<IEnumerable<Locale>> GetTranslationLocalesForFileAsync(Guid fileId)
        {
            return await this._filesService.GetTranslationLocalesForFileAsync(fileId: fileId);
        }

        [HttpPut("{fileId}/locales")]
        [Authorize]
        public async Task SetTranslationLocalesForFileAsync(Guid fileId, [FromBody] IEnumerable<Guid> localesIds)
        {
            await this._filesService.UpdateTranslationLocalesForFileAsync(
                fileId: fileId,
                localesIds: localesIds);
        }


        public class DownloadFileAsyncParam
        {
            public Guid? localeId { get; set; }
        }


        [HttpPost("{fileId}/download/{localeId}")]
        [Authorize]
        public async Task<FileResult> DownloadFileAsync(Guid fileId, DownloadFileAsyncParam param)
        {
            var fileStream = await this._filesService.GetFileAsync(fileId, param?.localeId);
            return this.File(
                fileStream: fileStream,
                contentType: "application/octet-stream",
                enableRangeProcessing: true);
        }

        [HttpPost("{fileId}/GetTranslationInfo")]
        [Authorize]
        public async Task<IEnumerable<FileTranslationInfo>> GetFileTranslationInfoAsync(Guid fileId)
        {


            return await this._filesService.GetFileTranslationInfoAsync(fileId: fileId);

        }

    }

}
