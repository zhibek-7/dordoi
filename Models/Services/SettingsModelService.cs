using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Models.Models;

namespace Models.Services
{
    public class SettingsModelService
    {

        private readonly string _settingsDefaultFileName = @"appsettings.json";

        public SettingsModel DeserializeFromDefaultLocation()
        {
            return this.DeserializeFromFIle(this._settingsDefaultFileName);
        }

        public SettingsModel DeserializeFromFIle(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                stream.Position = 0;
                var serializer = new DataContractJsonSerializer(
                    type: typeof(SettingsModel),
                    settings:
                        new DataContractJsonSerializerSettings()
                        {
                            UseSimpleDictionaryFormat = true,
                        });
                var settingsModel = serializer.ReadObject(stream) as SettingsModel;
                if (settingsModel == null)
                {
                    throw new Exception($@"Не удалось прочитать настройки из файла ""{filePath}"".");
                }
                return settingsModel;
            }
        }

    }
}
