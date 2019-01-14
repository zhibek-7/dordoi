﻿using DAL.Reposity.PostgreSqlRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.DTO;
using Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlossariesController : ControllerBase
    {
        private readonly GlossariesService _glossariesService;

        private readonly LocaleRepository _localeRepository = new LocaleRepository();
        private readonly LocalizationProjectRepository _localizationProjectRepository = new LocalizationProjectRepository();

        public GlossariesController(GlossariesService glossariesService 
            //, LocaleRepository localeRepository, LocalizationProjectRepository localizationProjectRepository
            )
        {
            _glossariesService = glossariesService;
            //_localeRepository = localeRepository;
            //_localizationProjectRepository = localizationProjectRepository;

            //_localeRepository = new LocaleRepository();
            //_localizationProjectRepository = new LocalizationProjectRepository();
        }

        //[HttpPost]
        //public async Task<IEnumerable<Glossaries>> GetAllAsync()
        //{
        //    return await _glossariesService.GetAllAsync();
        //}

        [HttpGet]
        public async Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync() //Переименовать в GetAllDTOAsync
        {
            return await _glossariesService.GetAllToDTOAsync(); //Переименовать в GetAllDTOAsync
        }

        [HttpGet("locales/list")]
        public async Task<IEnumerable<Locale>> GetLocalesAsync()
        {
            return await _localeRepository.GetAllAsync();
        }

        [HttpGet("localizationProjects/list")]
        public async Task<IEnumerable<localizationProjectForSelectDTO>> GetLocalizationProjectsAsync()
        {
            return await _localizationProjectRepository.GetAllForSelectDTOAsync();
        }
    }
}