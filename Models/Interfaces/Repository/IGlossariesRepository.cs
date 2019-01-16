using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DTO;

namespace Models.Interfaces.Repository
{
    public interface IGlossariesRepository : IBaseRepositoryAsync<Glossaries>
    {
        Task<IEnumerable<GlossariesTableViewDTO>> GetAllToDTOAsync();
        Task AddNewGlossaryAsync(GlossariesForEditing glossary);
        Task DeleteGlossaryAsync(int id);
    }
}
