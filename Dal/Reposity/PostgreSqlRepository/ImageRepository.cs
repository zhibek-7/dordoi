using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Models.DatabaseEntities;
using DAL.Context;
using Models.Interfaces.Repository;
using Npgsql;
using System.Threading.Tasks;

namespace DAL.Reposity.PostgreSqlRepository
{
    /// <summary>
    ///
    /// </summary>
    public class ImageRepository : BaseRepository, IImagesRepository, IBaseRepositoryAsync<Image>
    {
        private PostgreSqlNativeContext context;
        public ImageRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<IEnumerable<Image>> GetAllAsync(int? userId, int? projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Ограничение выдачи по пользователю и в рамках проекта
                    var sqlString = @"SELECT i.id, i.name_text, i.date_time_added, i.id_user, i.body, i.url
FROM public.images as i
inner join  comments_images as ci
 on i.id = ci.id_image
inner join  comments_text as ct
 on ci.id_comment = ct.id
inner join translation_substrings as ts
on ct.id_translation_substrings = ts.id
inner join files as f
	on ts.id_file_owner = f.id
inner join localization_projects as lp
	on f.id_localization_project = lp.id
inner join participants as p
	on lp.id = p.id_localization_project
where active = true and lp.id = " + (int)projectId + @" and   p.id_user =" + (int)userId + @" --- подставляется значение
union
SELECT i.id, i.name_text, i.date_time_added, i.id_user, i.body, i.url
FROM public.images as i
inner join  strings_context_images as ci
 on i.id = ci.id_image
inner join translation_substrings as ts
on ci.id_string = ts.id
inner join files as f
	on ts.id_file_owner = f.id
inner join localization_projects as lp
	on f.id_localization_project = lp.id
inner join participants as p
	on lp.id = p.id_localization_project
where  active = true and  lp.id = " + (int)projectId + @" and   p.id_user =" + (int)userId + @" --- подставляется значение
order by name_text";

                    this.LogQuery(sqlString);
                    IEnumerable<Image> images = dbConnection.Query<Image>(sqlString).ToList();
                    return images;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в { nameof(ImageRepository)}.{ nameof(ImageRepository.GetAllAsync)}{ nameof(NpgsqlException)}", exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }

        }

        public async Task<bool> UpdateAsync(Image image)
        {
            var sql = "update images set " +
                "name_text = @Name_text," +
                "date_time_added = @Date_Time_Added," +
                "id_user = @ID_User," +
                "body = @body," +
                "url = @URL " +
                "where id = @id";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(sql, image);
                    await dbConnection.ExecuteAsync(sql, image);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в { nameof(ImageRepository)}.{ nameof(ImageRepository.UpdateAsync)}{ nameof(NpgsqlException)}", exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }

    }
}
