using System.Collections.Generic;
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
                    Name = t.FirstOrDefault().Name,

                    LocalesName = string.Join(", ", t.Select(x => x.LocaleName).Distinct().OrderBy(n => n)),
                    LocalizationProjectsName = string.Join(", ",
                        t.Select(x => x.LocalizationProjectName).Distinct().OrderBy(n => n))
                }).OrderBy(t => t.Name);
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
                    ID_LocalizationProject = glossary.LocalizationProjectsIds.First(x => x.HasValue).Value,
                    Name = glossary.Name,
                    DateOfChange = DateTime.Now,
                    IsFolder = false,
                    IsLastVersion = true,
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
                    Name = temp.FirstOrDefault().Name,
                    Description = temp.FirstOrDefault().Description,
                    ID_File = temp.FirstOrDefault().ID_File,
                    LocalesIds = temp.Select(t => t.LocaleID).Distinct(),
                    LocalizationProjectsIds = temp.Select(t => t.LocalizationProjectID).Distinct()
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
