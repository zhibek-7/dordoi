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
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllByUserIdAsync(
            Guid? userId,
            int offset,
            int limit,
            Guid? projectId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            try
            {
                return await _glossariesRepository.GetAllByUserIdAsync(userId, offset, limit, projectId, searchString, sortBy, sortAscending);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Возвращает количество глоссариев.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия глоссария (поиск по name_text).</param>
        /// <returns></returns>
        public async Task<int> GetAllByUserIdCountAsync(
            Guid? userId,
            Guid? projectId = null,
            string searchString = null)
        {
            try
            {
                return await _glossariesRepository.GetAllByUserIdCountAsync(userId, projectId, searchString);
            }
            catch (Exception exception)
            {
                 throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Возвращает глоссарий для редактирования (со связанными объектами).
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task<GlossariesForEditingDTO> GetGlossaryForEditAsync(Guid glossaryId)
        {
            try
            {
                var temp = await _glossariesRepository.GetGlossaryForEditAsync(glossaryId);
                //Создание глоссария с вложенными списками идентификаторов связанных данных.
                var resultDTO = new GlossariesForEditingDTO
                {
                    id = temp.FirstOrDefault().id,
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
                throw new Exception(WriteLn($"Error", exception), exception);
            }
        }

        /// <summary>
        /// Добавление нового глоссария.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="glossary">Новый глоссарий.</param>
        /// <returns></returns>
        public async Task AddAsync(Guid userId, GlossariesForEditingDTO glossary)
        {
            try
            {
                await _glossariesRepository.AddAsync(userId, glossary);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error", exception), exception);
            }
        }

        /// <summary>
        /// Сохранение изменений в глоссарии.
        /// </summary>
        /// <param name="glossary">Отредактированный глоссарий.</param>
        /// <returns></returns>
        public async Task UpdateAsync(Guid userId,GlossariesForEditingDTO glossary)
        {
            try
            {
                await _glossariesRepository.UpdateAsync(userId,glossary);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error", exception), exception);
            }
        }

        /// <summary>
        /// Удаление глоссария.
        /// </summary>
        /// <param name="id">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _glossariesRepository.DeleteAsync(id);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error", exception), exception);
            }
        }

        /// <summary>
        /// Удаление всех терминов глоссария.
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария.</param>
        /// <returns></returns>
        public async Task ClearAsync(Guid glossaryId)
        {
            try
            {
                await _glossaryService.DeleteTermsByGlossaryAsync(glossaryId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn($"Error", exception), exception);
            }
        }

    }
}
