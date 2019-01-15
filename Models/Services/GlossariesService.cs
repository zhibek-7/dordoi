using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Interfaces.Repository;
using Models.DatabaseEntities;
using Models.DTO;

namespace Models.Services
{
    public class GlossariesService
    {
        private readonly IGlossariesRepository _glossariesRepository;

        public GlossariesService(IGlossariesRepository glossariesRepository)
        {
            _glossariesRepository = glossariesRepository;
        }


        //public async Task<IEnumerable<Glossaries>> GetAllAsync()
        //{
        //    return await _glossariesRepository.GetAllAsync();
        //}

        public async Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync()
        {
            return await _glossariesRepository.GetAllToDTOAsync();
        }

        public async Task AddNewGlossaryAsync(GlossariesForEditing glossary)
        {
            await _glossariesRepository.AddNewGlossaryAsync(glossary);
        }
    }
}
