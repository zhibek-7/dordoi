using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;

namespace Models.Interfaces.Repository
{
    public interface ITranslationTroubleRepository : IRepositoryAsync<TranslationTrouble>
    {
        Task<IEnumerable<TranslationTrouble>> GetByTranslationIdAsync(Guid translationId);
    }
}
