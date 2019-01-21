using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DTO;
using Models.Interfaces.Repository;

namespace Models.Services
{
    public class TranslationsTroublesService
    {
        private readonly ITranslationTroubleRepository _translationTroubleRepository;

        public TranslationsTroublesService(ITranslationTroubleRepository translationTroubleRepository)
        {
            _translationTroubleRepository = translationTroubleRepository;
        }

        public async Task<IEnumerable<TranslationTrouble>> GetAllByTranslationIdAsync(int traslationId)
        {
            return await _translationTroubleRepository.GetByTranslationIdAsync(traslationId);
        }

        public async Task<int> AddNewGlossaryAsync(TranslationTrouble trouble)
        {
            return await _translationTroubleRepository.AddAsync(trouble);
        }

        public async Task<bool> DeleteTranslationTroubleAsync(int id)
        {
            return await _translationTroubleRepository.RemoveAsync(id);
        }


    }
}
