using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;

namespace LocalizationServiceWpfApp
{
    public class TranslationSubstring
    {
        public long ID { get; set; }
        public string SubstringToTranslate { get; set; }
        public string Description { get; set; }
        public string Context { get; set; }
        public int? TranslationMaxLength { get; set; }
        public int ID_FileOwner { get; set; }
        public string Value { get; set; }
        public int PositionInText { get; set; }

        /// <summary>
        /// Конструктор для выгрузки из БД
        /// </summary>
        public TranslationSubstring(long ID, string SubstringToTranslate, string Description, string Context, int? TranslationMaxLength, int ID_FileOwner, string Value, int PositionInText)
        {
            this.ID = ID;
            this.SubstringToTranslate = SubstringToTranslate;
            this.Description = Description;
            this.Context = Context;
            this.TranslationMaxLength = TranslationMaxLength;
            this.ID_FileOwner = ID_FileOwner;
            this.Value = Value;
            this.PositionInText = PositionInText;
        }

        /// <summary>
        /// Конструктор для загрузки в БД
        /// </summary>
        /// <param name="substringToTranslate">Подстрока для перевода</param>
        /// <param name="description">Описание</param>
        /// <param name="stringContext">Контекст для перевода</param>
        /// <param name="id_FileOwner">ID файла-владельца строки</param>
        /// <param name="value">Подстрока перевода</param>
        /// <param name="positionInText">Позиция подстроки перевода в строке-владельце</param>
        public TranslationSubstring(NpgsqlConnection connection, string substringToTranslate, string context, int id_FileOwner, string value, int positionInText)
        {
            this.SubstringToTranslate = substringToTranslate;
            this.Description = null;
            this.Context = context;
            this.TranslationMaxLength = null;
            this.ID_FileOwner = id_FileOwner;
            this.Value = value;
            this.PositionInText = positionInText;
            NpgsqlCommand comm = new NpgsqlCommand("INSERT INTO \"TranslationSubstrings\" (\"SubstringToTranslate\", \"Context\", \"ID_FileOwner\", \"Value\", \"PositionInText\") VALUES (@SubstringToTranslate, @Context, @ID_FileOwner, @Value, @PositionInText) RETURNING \"ID\"", connection);
            comm.Parameters.AddWithValue("@SubstringToTranslate", this.SubstringToTranslate);
            comm.Parameters.AddWithValue("@Context", this.Context);
            comm.Parameters.AddWithValue("@ID_FileOwner", this.ID_FileOwner);
            comm.Parameters.AddWithValue("@Value", this.Value);
            comm.Parameters.AddWithValue("@PositionInText", this.PositionInText);
            this.ID = (long)comm.ExecuteScalar();
        }
    }
}
