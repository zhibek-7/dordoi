using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Models.Migration;
using Models.Models;
using Models.Services;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataImportController : Controller
    {

        private readonly ILocaleRepository _localeRepository;

        private readonly FromExcel _fromExcel;

        public DataImportController(ILocaleRepository localeRepository, FromExcel fromExcel)
        {
            this._localeRepository = localeRepository;
            this._fromExcel = fromExcel;
        }

        [HttpPost("locales")]
        public async Task ImportLocalesFromFile(IFormFile file, [FromForm] bool cleanTableBeforeImportFlag)
        {
            var parsedLocales = new Locale[] { };
            using (var fileStream = file.OpenReadStream())
            {
                parsedLocales = this._fromExcel.GetLocalesFromExcel(fileStream);
            }

            if (cleanTableBeforeImportFlag)
            {
                await this._localeRepository.CleanTableAsync();
            }

            foreach(var locale in parsedLocales)
            {
                await this._localeRepository.AddAsync(locale);
            }
        }

    }

}
