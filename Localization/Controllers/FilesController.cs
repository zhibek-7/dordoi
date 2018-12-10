using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.DatabaseEntities;
using Models.Models;
using Utilities.Extensions;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly IFilesRepository filesRepository;

        public FilesController(IFilesRepository filesRepository)
        {
            this.filesRepository = filesRepository;
        }

        // GET api/files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node<File>>>> GetAll()
        {
            var files = await filesRepository.GetAll();

            if (files == null)
            {
                return BadRequest("Files not found");
            }

            var tree = files.ToTree((file, icon) => new Node<File>(file, icon), (file) => GetIconByFile(file));

            return Ok(tree);
        }

        // GET api/files/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<File>> Get(int id)
        {
            var foundedFile = await filesRepository.GetByID(id);

            return Ok(foundedFile);
        }

        // POST api/files/add/file
        [HttpPost("add/file")]
        public async Task<ActionResult<Node<File>>> AddFile(IFormFile file, [FromForm] int? parentId)
        {
            // Check if file by id exists in database
            var foundedFile = await filesRepository.GetByNameAndParentId(file.FileName, parentId);

            if (foundedFile != null)
            {
                return BadRequest($"File \"{file.FileName}\" already exists");
            }

            var random = new Random();

            var addedFile = new File
            {
                Name = file.FileName,
                StringsCount = random.Next(500),
                Version = 0,
                Priority = 0,
                IsFolder = false,
                ID_FolderOwner = parentId,
            };

            return await AddNode(addedFile);
        }

        public class FolderModel
        {
            public string Name { get; set; }
            public int? ParentId { get; set; }   
        }

        // POST api/files/add/folder
        [HttpPost("add/folder")]
        public async Task<ActionResult<Node<File>>> AddFolder([FromBody] FolderModel model)
        {
            // Check if file by id exists in database
            var foundedFolder = await filesRepository.GetByNameAndParentId(model.Name, model.ParentId);

            if (foundedFolder != null)
            {
                return BadRequest($"Folder \"{model.Name}\" already exists");
            }

            // Create new file data object
            var addedFolder = new File
            {
                Name = model.Name,
                IsFolder = true,
                ID_FolderOwner = model.ParentId
            };

            return await AddNode(addedFolder);
        }

        // PUT api/files/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateNode(int id, File file)
        {
            // Check if file by id exists in database
            // var foundedFile = await filesRepository.GetByID(id);

            // if (foundedFile == null)
            // {
            //     return NotFound($"File by id \"{id}\" not found");
            // }

            // Update changed date to now 
            file.DateOfChange = DateTime.Now;

            // Update file in database
            var updateResult = await filesRepository.Update(file);

            if (!updateResult)
            {
                return BadRequest($"Failed to update file with id \"{id}\" in database");
            }

            // Return ok result
            return Ok();
        }

        // DELETE api/files/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNode(int id)
        {
            // Check if file by id exists in database
            // var foundedFile = await filesRepository.GetByID(id);

            // if (foundedFile == null)
            // {
            //     return NotFound($"File by id \"{id}\" not found");
            // }

            // Delete file from database
            var deleteResult = await filesRepository.Remove(id);

            if (!deleteResult)
            {
                return BadRequest($"Failed to remove file with id \"{id}\" from database");
            }

            // Return ok result
            return Ok();
        }

        /// <summary>  
        ///  This class performs an important function.  
        /// </summary> 
        public async Task<ActionResult<Node<File>>> AddNode(File file)
        {
            // Find parent node by parent node ID
            if (file.ID_FolderOwner.HasValue)
            {
                var parentFile = await filesRepository.GetByID(file.ID_FolderOwner.Value);

                if (parentFile?.IsFolder == false)
                {
                    return BadRequest($"Can not add node \"{file.Name}\" in file node");
                }
            }

            // Insert file in database 
            var affectedRows = await filesRepository.Add(file);

            if (affectedRows == 0)
            {
                return BadRequest($"Failed to insert folder \"{file.Name}\" in database");
            }

            // Get inserted file from database
            var addedFile = await filesRepository.GetByNameAndParentId(file.Name, file.ID_FolderOwner);

            // Get icon by data
            //var icon = GetIconByFile(addedFile);

            // Create new node object based on added file data
            var node = new Node<File>(addedFile);

            // Return node object
            return Ok(node);
        }

        private string GetIconByFile(File file)
        {
            // Icons section in JSON configuration
            //var iconsSection = configuration.GetSection("icons");

            //// If node is folder, return folder icon 
            //if (file.IsFolder)
            //{
            //    // var iconBundle = iconsSection.GetSection("folder").Get<IconBundle>();

            //    return iconsSection.GetValue<string>("folder");
            //}

            //// Get file extension by file name
            //var extension = System.IO.Path.GetExtension(file.Name);

            //// Get font-awesome icon class by file extension
            //return iconsSection.GetValue(extension, "assets/icons/file.png");

            return null;
        }
    }

}
