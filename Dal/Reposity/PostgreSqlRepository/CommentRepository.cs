using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Models.DatabaseEntities;
using Models.Comments;
using DAL.Context;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logs;
using Models.Interfaces.Repository;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class CommentRepository : BaseRepository, IRepositoryAsync<Comments>
    {
        private PostgreSqlNativeContext context;

        public CommentRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        /// <summary>
        /// Добавить комментарий
        /// </summary>
        /// <param name="comment">комментарий</param>
        /// <returns></returns>
        public async Task<int> AddAsync(Comments comment)
        {
            var query = "INSERT INTO \"Comments\" (\"ID_TranslationSubstrings\", \"DateTime\", \"ID_User\", \"Comment\")" +
                        "VALUES (@ID_TranslationSubstrings, @DateTime, @ID_User, @Comment) " +
                        "RETURNING  \"Comments\".\"ID\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<int>(query, comment);
                    dbConnection.Close();
                    return idOfInsertedRow;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return 0;
            }
        }

        /// <summary>
        /// Получить все комментарии
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Comments>> GetAllAsync()
        {
            var query = "SELECT * FROM \"Comments\"";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    IEnumerable<Comments> comments = await dbConnection.QueryAsync<Comments>(query);
                    dbConnection.Close();
                    return comments;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

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
            var query = "SELECT \"Users\".\"ID\" AS \"UserId\", \"Users\".\"Name\" AS \"UserName\"," +
                        " \"Comments\".\"ID\" AS \"CommentId\", \"Comments\".\"DateTime\" AS \"DateTime\"," +
                        " \"Comments\".\"Comment\" AS \"Comment\" " +
                        "FROM \"Comments\" " +
                        "INNER JOIN \"Users\" ON \"Comments\".\"ID_User\" = \"Users\".\"ID\" " +
                        "WHERE \"Comments\".\"ID_TranslationSubstrings\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var comments = await dbConnection.QueryAsync<CommentWithUserInfo>(query, new { Id = idString });
                    dbConnection.Close();
                    return comments;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

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
            var query = "SELECT * FROM \"Comments\" WHERE \"ID\" = @id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var comment = await dbConnection.QuerySingleOrDefaultAsync<Comments>(query, new { id });
                    dbConnection.Close();

                    return comment;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);


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
            var query = "SELECT \"Users\".\"ID\" AS \"UserId\", \"Users\".\"Name\" AS \"UserName\"," +
                        " \"Comments\".\"ID\" AS \"CommentId\", \"Comments\".\"DateTime\" AS \"DateTime\"," +
                        " \"Comments\".\"Comment\" AS \"Comment\" " +
                        "FROM \"Comments\" " +
                        "INNER JOIN \"Users\" ON \"Comments\".\"ID_User\" = \"Users\".\"ID\" " +
                        "WHERE \"Comments\".\"ID\" = @Id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var comment = await dbConnection.QuerySingleOrDefaultAsync<CommentWithUserInfo>(query, new { id });
                    dbConnection.Close();

                    return comment;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);

                return null;
            }
        }

        /// <summary>
        /// Удалить комментарий по id 
        /// </summary>
        /// <param name="id">id комментарий который нужно удалить</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(int id)
        {
            var query = "DELETE " +
                        "FROM \"Comments\" AS C " +
                        "WHERE C.\"ID\" = @id";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var deletedRows = await dbConnection.ExecuteAsync(query, new { id });
                    dbConnection.Close();

                    return deletedRows > 0;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);
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
            var query = "UPDATE \"Comments\" SET " +
                        "\"DateTime\"=@DateTime, " +
                        "\"ID_User\"=@ID_User, " +
                        "\"Comment\"=@Comment " +
                        "WHERE \"ID\"=@ID";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();

                    await dbConnection.ExecuteAsync(query, comment);

                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exception)
            {
                //Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        /// <summary>
        /// Загрузить картинку в базу данных
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task<int> UploadImageAsync(Byte[] image)
        {
            //ЗАПРОС ЕЩЕ НЕ НАПИСАН
            var query = "";

            try
            {
                using (IDbConnection dbConnection = context.Connection)
                {
                    dbConnection.Open();
                    var idOfInsertedRow = await dbConnection.ExecuteScalarAsync<int>(query, image);
                    dbConnection.Close();
                    return idOfInsertedRow;
                }
            }
            catch (Exception exception)
            {
                // Внесение записи в журнал логирования
                Console.WriteLine(exception.Message);
                return 0;
            }
        }

    }
}
