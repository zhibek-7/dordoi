using System.Threading.Tasks;
using Localization.Controllers;
using Localization.Hubs.DataImport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.DatabaseEntities;
using Models.Interfaces.Repository;
using Models.Migration;

namespace Localization.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataImportController : ControllerBase
    {

        private readonly ILocaleRepository _localeRepository;

        private readonly FromExcel _fromExcel;

        private readonly IHubContext<DataImportHub> _hubContext;

        public DataImportController(ILocaleRepository localeRepository, FromExcel fromExcel, IHubContext<DataImportHub> hubContext)
        {
            this._localeRepository = localeRepository;
            this._fromExcel = fromExcel;
            this._hubContext = hubContext;
        }

        [HttpPost("locales")]
        public async Task ImportLocalesFromFile(IFormFile file, [FromForm] bool cleanTableBeforeImportFlag, [FromForm] string signalrConnectionId)
        {
            var signalrHubClientProxy = this._hubContext.Clients.Client(signalrConnectionId);
            var parsedLocales = new Locale[] { };
            using (var fileStream = file.OpenReadStream())
            {
                parsedLocales = this._fromExcel.GetLocalesFromExcel(fileStream);
                await signalrHubClientProxy.Log($"Загруженный файл распарсен, количество записей - {parsedLocales.Length}.");
            }

            if (cleanTableBeforeImportFlag)
            {
                await signalrHubClientProxy.Log("Очистка таблицы перед импортом локалей...");
                await this._localeRepository.CleanTableAsync();
                await signalrHubClientProxy.Log("Выполнена очистка таблицы перед импортом локалей.");
            }

            foreach (var locale in parsedLocales)
            {
                var isLocaleAdded = await this._localeRepository.AddAsync(locale);
                if (isLocaleAdded)
                {
                    await signalrHubClientProxy.Log($"Добавлена локаль \"{locale.name_text}\".");
                }
                else
                {
                    await signalrHubClientProxy.Log($"Не удалось добавить локаль \"{locale.name_text}\".");
                }
            }
            await signalrHubClientProxy.Log("Добавление локалей завершено.");
        }

    }

}
