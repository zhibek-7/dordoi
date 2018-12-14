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

namespace DAL.Reposity.PostgreSqlRepository
{
    public class CommentRepository : IRepositoryAsync<Comments>
    {
        private PostgreSqlNativeContext context;

        public CommentRepository()
        {
            context = PostgreSqlNativeContext.getInstance();
        }

        public Task<int> AddAsync(Comments item)
        {
            throw new NotImplementedException();
        }

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

        public Task<bool> UpdateAsync(Comments item)
        {
            throw new NotImplementedException();
        }
    }
}
