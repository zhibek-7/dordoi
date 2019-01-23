using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Interfaces.Repository;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using System.Linq;

namespace Models.Services
{
    public class GlossariesService
    {
        private readonly IGlossariesRepository _glossariesRepository;

        private readonly GlossaryService _glossaryService;


        public GlossariesService(IGlossariesRepository glossariesRepository, GlossaryService glossaryService)
        {
            _glossariesRepository = glossariesRepository;
            _glossaryService = glossaryService;
        }


        //public async Task<IEnumerable<Glossaries>> GetAllAsync()
        //{
        //    return await _glossariesRepository.GetAllAsync();
        //}

        public async Task<IEnumerable<GlossariesTableViewDTO>> GetAllToDTOAsync()
        {
            var temp = await _glossariesRepository.GetAllAsync();
            var resultDTO = temp.GroupBy(t => t.ID).Select(t => new GlossariesTableViewDTO
            {
                ID = t.Key,
                Name = t.FirstOrDefault().Name,
                //ID_File = t.FirstOrDefault().ID_File,

                LocalesName = string.Join(", ", t.Select(x => x.LocaleName).Distinct().OrderBy(n => n)),
                LocalizationProjectsName = string.Join(", ", t.Select(x => x.LocalizationProjectName).Distinct().OrderBy(n => n))
            }).OrderBy(t => t.Name);
            return resultDTO;
        }

        public async Task AddNewGlossaryAsync(GlossariesForEditing glossary)
        {
            await _glossariesRepository.AddNewGlossaryAsync(glossary);
        }

        public async Task<GlossariesForEditing> GetGlossaryForEditAsync(int glossaryId)
        {
            return await _glossariesRepository.GetGlossaryForEditAsync(glossaryId);
        }

        public async Task EditGlossaryAsync(GlossariesForEditing glossary)
        {
            await _glossariesRepository.EditGlossaryAsync(glossary);
        }

        public async Task DeleteGlossaryAsync(int id)
        {
            await _glossariesRepository.DeleteGlossaryAsync(id);
        }

        /// <summary>
        /// Удаление всех терминов глоссария
        /// </summary>
        /// <param name="glossaryId">Идентификатор глоссария</param>
        /// <returns></returns>
        public async Task ClearGlossaryAsync(int id)
        {
            await _glossaryService.DeleteTermsByGlossaryAsync(id);
        }

    }
}
