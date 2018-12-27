using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using Models.DTO;
using Models.Interfaces.Repository;

namespace Models.Services
{
    public class GlossariesService
    {
        private readonly IGlossariesRepository _glossariesRepository;

        public GlossariesService(IGlossariesRepository glossariesRepository)
        {
            this._glossariesRepository = glossariesRepository;
        }


        public async Task<IEnumerable<DTO.Glossaries>> GetAllAsync()
        {
            return await this._glossariesRepository.GetAllAsync();
        }
        
        public async Task<IEnumerable<DTO.GlossariesDTO>> GetAllToDTOAsync()
        {
            return await this._glossariesRepository.GetAllToDTOAsync();
        }
    }
}
