using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DTO;

namespace Models.Interfaces.Repository
{
    public interface IGlossariesRepository : IBaseRepositoryAsync<Glossaries>
    {
        Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync();
        Task AddNewGlossaryAsync(GlossariesForEditing glossary);
    }
}
