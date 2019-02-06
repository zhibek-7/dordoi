﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Interfaces.Repository;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using System.Linq;
using System;

namespace Models.Services
{
    public class GlossariesService : BaseService
    {
        private readonly IGlossariesRepository _glossariesRepository;

        private readonly GlossaryService _glossaryService;

        private readonly IFilesRepository _filesRepository;

        public GlossariesService(IGlossariesRepository glossariesRepository, GlossaryService glossaryService, IFilesRepository filesRepository)
        {
            _glossariesRepository = glossariesRepository;
            _glossaryService = glossaryService;
            this._filesRepository = filesRepository;
        }


        /// <summary>
        /// Возвращает список глоссариев, со строками перечислений имен связанных объектов.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllToDTOAsync()
        {
            try
            {
                var temp = await _glossariesRepository.GetAllAsync();
                //Создание списка глоссариев со строками перечислений имен связанных объектов.
                var resultDTO = temp.GroupBy(t => t.ID).Select(t => new GlossariesTableViewDTO
                {
                    ID = t.Key,
                    Name_text = t.FirstOrDefault().Name_text,

                    Locales_Name = string.Join(", ", t.Select(x => x.Locale_Name).Distinct().OrderBy(n => n)),
                    Localization_Projects_Name = string.Join(", ",
                        t.Select(x => x.Localization_Project_Name).Distinct().OrderBy(n => n))
                }).OrderBy(t => t.Name_text);
                return resultDTO;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error", exception);
            }
        }

        /// <summary>
        /// Добавление нового глоссария.
        /// </summary>
        /// <param name="glossary">Новый глоссарий.</param>
        /// <returns></returns>
        public async Task AddNewGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            try
            {
                var newGlossaryFileId = await this._filesRepository.AddAsync(new File()
                {
                    ID_Localization_Project = glossary.Localization_Projects_Ids.First(x => x.HasValue).Value,
                    Name_text = glossary.Name_text,
                    Date_Of_Change = DateTime.Now,
                    Is_Folder = false,
                    Is_Last_Version = true,
                });
                glossary.ID_File = newGlossaryFileId;
                await _glossariesRepository.AddNewGlossaryAsync(glossary);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error", exception);
            }
        }

        /// <summary>
        /// Возвращает глоссарий для редактирования (со связанными объектами).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task<GlossariesForEditingDTO> GetGlossaryForEditAsync(int glossaryId)
        {
            try
            {
                var temp = await _glossariesRepository.GetGlossaryForEditAsync(glossaryId);
                //Создание глоссария с вложенными списками идентификаторов связанных данных.
                var resultDTO = new GlossariesForEditingDTO
                {
                    ID = temp.FirstOrDefault().ID,
                    Name_text = temp.FirstOrDefault().Name_text,
                    Description = temp.FirstOrDefault().Description,
                    ID_File = temp.FirstOrDefault().ID_File,
                    Locales_Ids = temp.Select(t => t.Locale_ID).Distinct(),
                    Localization_Projects_Ids = temp.Select(t => t.Localization_Project_ID).Distinct()
                };
                return resultDTO;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error", exception);
            }
        }

        /// <summary>
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий.</param>
        /// <returns></returns>
        public async Task EditGlossaryAsync(GlossariesForEditingDTO glossary)
        {
            try
            {
                await _glossariesRepository.EditGlossaryAsync(glossary);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error", exception);
            }
        }

        /// <summary>
        /// Удаление глоссария.
        /// </summary>
        /// <param name="id">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task DeleteGlossaryAsync(int id)
        {
            try
            {
                await _glossariesRepository.DeleteGlossaryAsync(id);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error", exception);
            }
        }

        /// <summary>
        /// Удаление всех терминов глоссария.
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task ClearGlossaryAsync(int id)
        {
            try
            {
                await _glossaryService.DeleteTermsByGlossaryAsync(id);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error", exception);
            }
        }

    }
}
