using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Interfaces.Repository;
using Models.DatabaseEntities.DTO;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Services
{
    public class TranslationMemoryService : BaseService
    {
        private readonly ITranslationMemoryRepository _translationMemoryRepository;

        private readonly IFilesRepository _filesRepository;
        private readonly ITranslationSubstringRepository _translationSubstringRepository;

        public TranslationMemoryService(ITranslationMemoryRepository translationMemoriesRepository, IFilesRepository filesRepository, ITranslationSubstringRepository translationSubstringRepository)
        {
            _translationMemoryRepository = translationMemoriesRepository;
            _filesRepository = filesRepository;
            _translationSubstringRepository = translationSubstringRepository;
        }

        /// <summary>
        /// Возвращает список памяти переводов, со строками перечислений имен связанных объектов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TranslationMemoryTableViewDTO>> GetAllByUserIdAsync(
            int? userId,
            int offset,
            int limit,
            int? projectId = null,
            string searchString = null,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            try
            {
                return await _translationMemoryRepository.GetAllByUserIdAsync(userId, offset, limit, projectId, searchString, sortBy, sortAscending);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Возвращает количество памятей переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <param name="searchString">Шаблон названия памяти переводов (поиск по name_text).</param>
        /// <returns></returns>
        public async Task<int> GetAllByUserIdCountAsync(
            int? userId,
            int? projectId = null,
            string searchString = null)
        {
            try
            {
                return await _translationMemoryRepository.GetAllByUserIdCountAsync(userId, projectId, searchString);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Возвращает список памятей переводов назначенных на проект локализации.
        /// </summary>
        /// <param name="projectId">Идентификатор проекта локализации.</param>
        /// <returns>TranslationMemoryForSelectDTO</returns>
        public async Task<IEnumerable<TranslationMemoryForSelectDTO>> GetForSelectByProjectAsync(int projectId)
        {
            return await _translationMemoryRepository.GetForSelectByProjectAsync(projectId);
        }

        /// <summary>
        /// Добавление новой памяти переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemory">Новая память переводов.</param>
        /// <returns></returns>
        public async Task AddAsync(int userId, TranslationMemoryForEditingDTO translationMemory)
        {
            try
            {
                var newTranslationMemoryFileId = await this._filesRepository.AddAsync(new File()
                {
                    id_localization_project = translationMemory.localization_projects_ids.First(x => x.HasValue).Value,
                    name_text = translationMemory.name_text,
                    date_of_change = DateTime.Now,
                    is_folder = false,
                    is_last_version = true,
                    visibility = false
                });
                translationMemory.id_file = newTranslationMemoryFileId;
                await _translationMemoryRepository.AddAsync(userId, translationMemory);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Возвращает память переводов для редактирования (со связанными объектами).
        /// </summary>
        /// <param name="translationMemoryId">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<TranslationMemoryForEditingDTO> GetForEditAsync(int translationMemoryId)
        {
            try
            {
                var temp = await _translationMemoryRepository.GetForEditAsync(translationMemoryId);
                //Создание памяти переводов с вложенными списками идентификаторов связанных данных.
                var resultDTO = new TranslationMemoryForEditingDTO
                {
                    id = temp.FirstOrDefault().id,
                    name_text = temp.FirstOrDefault().name_text,
                    id_file = temp.FirstOrDefault().id_file,
                    locales_ids = temp.Select(t => t.locale_id).Distinct(),
                    localization_projects_ids = temp.Select(t => t.localization_project_id).Distinct()
                };
                return resultDTO;
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Сохранение изменений в памяти переводов.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="translationMemory">Отредактированная память переводов.</param>
        /// <returns></returns>
        public async Task UpdateAsync(int userId, TranslationMemoryForEditingDTO translationMemory)
        {
            try
            {
                await _translationMemoryRepository.UpdateAsync(userId, translationMemory);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Удаление памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _translationMemoryRepository.DeleteAsync(id);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        /// <summary>
        /// Удаление всех строк памяти переводов.
        /// </summary>
        /// <param name="id">Идентификатор памяти переводов.</param>
        /// <returns></returns>
        public async Task<bool> ClearAsync(int id)
        {
            try
            {
                return await _translationSubstringRepository.RemoveByTranslationMemoryAsync(id);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }
    }
}
