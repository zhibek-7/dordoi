using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Models.DatabaseEntities;
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

        public ImageRepository(string connectionStr) : base(connectionStr)
        {
        }

        public async Task<IEnumerable<Image>> GetAllAsync(Guid? userId, Guid? projectId)
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
where active = true and lp.id = '" + (Guid)projectId + @"' and   p.id_user ='" + (Guid)userId + @"' --- подставляется значение
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
where  active = true and  lp.id = '" + (Guid)projectId + @"' and   p.id_user ='" + (Guid)userId + @"' --- подставляется значение
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

        public async Task<int> GetFilteredCountAsync(
            Guid userId,
            Guid projectId,
            string imageNameFilter
            )
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sql = this.GetFilteredImagesSql(imageNameFilter: imageNameFilter);
                    sql += "select count(*) from all_related_images";
                    if (!string.IsNullOrEmpty(imageNameFilter))
                    {
                        sql += " where name_text like @searchPattern";
                    }
                    var param = new
                    {
                        userId,
                        projectId,
                        searchPattern = $"%{imageNameFilter}%"
                    };

                    this.LogQuery(sql, param);
                    return await dbConnection.ExecuteScalarAsync<int>(sql, param);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в { nameof(ImageRepository)}.{ nameof(ImageRepository.GetFilteredCountAsync)}{ nameof(NpgsqlException)}", exception);
                return -1;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.GetFilteredCountAsync)} {nameof(Exception)} ",
                    exception);
                return -1;
            }
        }

        public async Task<IEnumerable<Image>> GetFilteredAsync(
            Guid userId,
            Guid projectId,
            string imageNameFilter,
            int limit,
            int offset
            )
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var sql = this.GetFilteredImagesSql(imageNameFilter: imageNameFilter);
                    sql += "select * from all_related_images";
                    if (!string.IsNullOrEmpty(imageNameFilter))
                    {
                        sql += " where name_text like @searchPattern";
                    }
                    sql += " order by name_text" +
                        " limit @limit" +
                        " offset @offset";
                    var param = new
                    {
                        userId,
                        projectId,
                        searchPattern = $"%{imageNameFilter}%",
                        limit,
                        offset
                    };

                    this.LogQuery(sql, param);
                    return await dbConnection.QueryAsync<Image>(sql, param);
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в { nameof(ImageRepository)}.{ nameof(ImageRepository.GetFilteredAsync)}{ nameof(NpgsqlException)}", exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(ImageRepository)}.{nameof(ImageRepository.GetFilteredAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        private string GetFilteredImagesSql(string imageNameFilter)
        {
            var getFilteredImagesSql =
               @"with all_related_images as (SELECT i.id, i.name_text, i.date_time_added, i.id_user, i.body, i.url
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
where active = true and lp.id = @projectId and p.id_user = @userId
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
where active = true and lp.id = @projectId and p.id_user = @userId)
";
            return getFilteredImagesSql;
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
