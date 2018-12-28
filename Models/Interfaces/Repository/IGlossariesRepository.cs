using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;
//using Models.DTO;

namespace Models.Interfaces.Repository
{
    public interface IGlossariesRepository : IRepositoryAsync<DTO.Glossaries>
    {
        //Не удалось расположить в GlossariesService, поэтому он здесь
        Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync();

        Task CleanOfTermsAsync(int id);
    }
}
