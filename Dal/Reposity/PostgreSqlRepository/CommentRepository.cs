﻿using System;
using System.Collections.Generic;
using Dapper;
using Models.DatabaseEntities;
using DAL.Context;
using System.Data;
using System.Threading.Tasks;
using Models.Interfaces.Repository;
using Npgsql;
using Models.DatabaseEntities.PartialEntities.Comment;
using System.Linq;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class CommentRepository : BaseRepository, IRepositoryAsync<Comments>
    {
        public CommentRepository(string connectionStr) : base(connectionStr)
        {
        }

        /// <summary>
        /// Добавить комментарий
        /// </summary>
        /// <param name="comment">комментарий</param>
        /// <returns></returns>
        public async Task<int> AddAsync(Comments comment)
        {
            var query = "INSERT INTO comments_text (id_translation_substrings, datetime, id_user, comment_text)" +
                        "VALUES (@ID_TranslationSubstrings, @DateTime, @ID_User, @Comment) " +
                        "RETURNING  comments_text.id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query, param: comment);
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<int>(query, comment);
                    return idOfInsertedRow;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }


        /// <summary>
        /// Добавить файл в комментарий
        /// </summary>
        /// <param name="comment">комментарий</param>
        /// <returns></returns>
        public async Task<int> AddFileAsync(Comments comment)
        {

            var query = "INSERT INTO images (name_text,  id_user, data, url) VALUES('test',  comment.ID_User,  '‰PNG','')";
            /*
                    var query = "INSERT INTO comments_text (\"ID_TranslationSubstrings\", datetime, id_user, comment_text)" +
                                    "VALUES (@ID_TranslationSubstrings, @DateTime, @ID_User, @Comment) " +
                                    "RETURNING  comments_text.id";
            */
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query, param: comment);
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<int>(query, comment);
                    return idOfInsertedRow;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }

        }

        /// <summary>
        /// Получить все комментарии
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Comments>> GetAllAsync()
        {
            var query = "SELECT * FROM comments_text";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query);
                    IEnumerable<Comments> comments = await dbConnection.QueryAsync<Comments>(query);
                    return comments;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetAllAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetAllAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Получает все комментарии которые есть данной фразы
        /// </summary>
        /// <param name="idString">id фразы, комментарии которой необходимы</param>
        /// <returns>Список комментариев</returns>        
        public async Task<IEnumerable<CommentWithUserInfo>> GetAllCommentsInStringByID(int idString)
        {
            var query = "SELECT users.id AS userid, users.name_text AS username," +
                        " comments_text.id AS commentid, comments_text.datetime AS datetime," +
                        " comments_text.comment_text AS comment_text " +
                        "FROM comments_text " +
                        "INNER JOIN users ON comments_text.id_user = users.id " +
                        "WHERE comments_text.id_translation_substrings = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { idString };
                    this.LogQuery(query, param);
                    var comments = await dbConnection.QueryAsync<CommentWithUserInfo>(query, param);
                    foreach(var comment in comments)
                    {
                        comment.Images = await GetImagesOfCommentAsync(comment.Comment_Id);
                    }
                    return comments;
                }
            }

            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetAllCommentsInStringByID)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetAllCommentsInStringByID)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Получить комментарий по id
        /// </summary>
        /// <param name="id">id комментария который нужно получить</param>
        /// <returns></returns>
        public async Task<Comments> GetByIDAsync(int id)
        {
            var query = "SELECT * FROM comments_text WHERE id = @id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(query, param);
                    var comment = await dbConnection.QuerySingleOrDefaultAsync<Comments>(query, param);
                    return comment;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetByIDAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetByIDAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Получить комментарий по id комментария с информацией о пользователе, который 
        /// добавил данный комментарий
        /// </summary>
        /// <param name="id">id комментария который нужно получить</param>
        /// <returns></returns>
        public async Task<CommentWithUserInfo> GetByIDWithUserInfoAsync(int id)
        {
            var query = "SELECT users.id AS userid, users.name_text AS username," +
                        " comments_text.id AS commentid, comments_text.datetime AS datetime," +
                        " comments_text.comment_text AS comment_text " +
                        "FROM comments_text " +
                        "INNER JOIN users ON comments_text.id_user = users.id " +
                        "WHERE comments_text.id = @Id";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param = new { id };
                    this.LogQuery(query, param);
                    var comment = await dbConnection.QuerySingleOrDefaultAsync<CommentWithUserInfo>(query, param);
                    return comment;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetByIDWithUserInfoAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.GetByIDWithUserInfoAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        /// <summary>
        /// Удалить комментарий по id 
        /// </summary>
        /// <param name="id">id комментарий который нужно удалить</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(int commentId)
        {
            //"" +
            //"DELETE " +
            //"FROM comments_images AS CI " +
            //"WHERE CI.id_comment = @id ";
            var query1 = "SELECT CI.id_image " +
                         "FROM comments_images AS CI " +
                         "WHERE CI.id_comment = @CommentId;";

            var query2 = "DELETE " +
                         "FROM comments_text AS C " +
                         "WHERE C.id = @CommentId; " +
                         "" +
                         "DELETE " +
                         "FROM images AS I " +
                         "WHERE I.id = @ImageId;";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var param1 = new { CommentId = commentId };
                    this.LogQuery(query1, param1);
                    var deletedImageId = await dbConnection.QueryAsync<int>(query1, param1);

                    var param2 = new { CommentId = commentId, ImageId = deletedImageId.ElementAt(0) };
                    this.LogQuery(query2, param2);
                    var deletedRows = await dbConnection.ExecuteAsync(query2, param2);
                    return deletedRows > 0;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.RemoveAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.RemoveAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }


        /// <summary>
        /// Обновить комментарий
        /// </summary>
        /// <param name="comment">обновленный комментарий</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Comments comment)
        {
            var query = "UPDATE comments_text SET " +
                        "datetime=@DateTime, " +
                        "id_user=@ID_User, " +
                        "comment_text=@Comment " +
                        "WHERE id=@ID";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query, comment);
                    await dbConnection.ExecuteAsync(query, comment);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.UpdateAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return false;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.UpdateAsync)} {nameof(Exception)} ",
                    exception);
                return false;
            }
        }


        /// <summary>
        /// Добавить изображение к комментарию
        /// </summary>
        /// <param name="img">Изображение</param>
        /// <param name="commentId">Id комментария</param>
        /// <returns></returns>
        public async Task<int> UploadImageAsync(Image img, int commentId)
        {
            var query1 = "INSERT INTO images (name_text, id_user, body, url)" +
                        "VALUES (@Name,  @ID_User, @Data, @url) " +
                        "RETURNING  images.id";

            var query2 = "INSERT INTO comments_images (id_comment, id_image)" +
                        "VALUES (@CommentId,  @ImageId) ";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query1, img);
                    var idOfInsertedImage = await dbConnection.ExecuteScalarAsync<int>(query1, img);

                    this.LogQuery(query2, commentId);
                    await dbConnection.ExecuteScalarAsync(query2, new { CommentId = commentId, ImageId = idOfInsertedImage });
                    return idOfInsertedImage;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return 0;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(Exception)} ",
                    exception);
                return 0;
            }
        }

        /// <summary>
        /// Получить все изображения прикрепленные к конкретному комментарию
        /// </summary>
        /// <param name="commentId">id комментария</param>
        /// <returns>Список изображений</returns>
        public async Task<IEnumerable<Image>> GetImagesOfCommentAsync(int commentId)
        {
            var query = "SELECT Im.id, Im.url, Im.name_text, Im.date_time_added, Im.body, Im.id_user " +
                        "FROM images AS Im " +
                        "INNER JOIN comments_images AS CI ON CI.id_image = Im.id " +
                        "INNER JOIN comments_text AS C ON C.id = CI.id_comment " +
                        "WHERE C.id = @CommentId ";

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(query, commentId);
                    IEnumerable<Image> images = await dbConnection.QueryAsync<Image>(query, new { CommentId = commentId });
                    foreach (var image in images)
                    {
                        image.URL = Convert.ToBase64String(image.body);
                    }
                    return images;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(CommentRepository)}.{nameof(CommentRepository.AddFileAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

    }
}
