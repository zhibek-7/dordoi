using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;


using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface IGlossariesRepository : IBaseRepositoryAsync<Glossaries>
    {
        Task<IEnumerable<GlossariesDTO>> GetAllToDTOAsync();
    }
}
