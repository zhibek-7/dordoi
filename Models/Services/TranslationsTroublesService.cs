using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.DatabaseEntities;
using Models.DatabaseEntities.DTO;
using Models.Interfaces.Repository;

namespace Models.Services
{
    public class TranslationsTroublesService : BaseService
    {
        private readonly ITranslationTroubleRepository _translationTroubleRepository;

        public TranslationsTroublesService(ITranslationTroubleRepository translationTroubleRepository)
        {


            try
            {
                _translationTroubleRepository = translationTroubleRepository;
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        public async Task<IEnumerable<TranslationTrouble>> GetAllByTranslationIdAsync(int traslationId)
        {


            try
            {
                return await _translationTroubleRepository.GetByTranslationIdAsync(traslationId);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }

        }

        public async Task<int> AddNewGlossaryAsync(TranslationTrouble trouble)
        {


            try
            {
                return await _translationTroubleRepository.AddAsync(trouble);
            }
            catch (Exception exception)
            {
                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }

        public async Task<bool> DeleteTranslationTroubleAsync(int id)
        {


            try
            {
                return await _translationTroubleRepository.RemoveAsync(id);
            }
            catch (Exception exception)
            {

                throw new Exception(WriteLn(exception.Message, exception), exception);
            }
        }


    }
}
