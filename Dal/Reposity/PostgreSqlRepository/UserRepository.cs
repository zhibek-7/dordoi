using System;
using System.Collections.Generic;
using System.Data;
using Models.DatabaseEntities;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using SqlKata;
using Models.Interfaces.Repository;
using Npgsql;
using Models.DatabaseEntities.DTO;

namespace DAL.Reposity.PostgreSqlRepository
{
    public class UserRepository : BaseRepository, IUserRepository, IRepository<User>
    {
        private readonly ParticipantRepository _participantsRepository;
        private RoleRepository roles;
        private ParticipantRepository participant;
        public UserRepository(string connectionStr) : base(connectionStr)
        {
            _participantsRepository = new ParticipantRepository(connectionStr);

            roles = new RoleRepository(connectionStr);
            participant = new ParticipantRepository(connectionStr);
        }

        //public void Add(User user)
        //{
        //    try
        //    {

        //        using (var dbConnection = new NpgsqlConnection(connectionString))
        //        {
        //            string SQLQuery = "INSERT INTO users (name_text, password_text, photo, email) VALUES (@Name_text, @Password_text, @Photo, @Email)";
        //            this.LogQuery(SQLQuery, user.GetType(), user);
        //            dbConnection.Execute(SQLQuery, user);
        //        }

        //    }
        //    catch (NpgsqlException exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Add)} {nameof(NpgsqlException)} ",
        //            exception);
        //    }
        //    catch (Exception exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Add)} {nameof(Exception)} ",
        //            exception);
        //    }
        //}

        //public User GetByID(int Id)
        //{
        //    try
        //    {
        //        User user = null;
        //        string SQLQuery = "SELECT * FROM users WHERE Id = @Id";
        //        using (var dbConnection = new NpgsqlConnection(connectionString))
        //        {
        //            var param = new { Id };
        //            this.LogQuery(SQLQuery, param);
        //            user = dbConnection.Query<User>(SQLQuery, param).FirstOrDefault();
        //        }
        //        return user;
        //    }
        //    catch (NpgsqlException exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByID)} {nameof(NpgsqlException)} ",
        //            exception);
        //        return null;
        //    }
        //    catch (Exception exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByID)} {nameof(Exception)} ",
        //            exception);
        //        return null;
        //    }
        //}

        public IEnumerable<User> GetByProjectID(Guid Id)
        {
            try
            {
                string SQLQuery = @"SELECT u.* FROM users u
                                   join participants p on u.id = p.id_user 
                         join localization_projects lp on p.id_localization_project = lp.id 
                         where lp.id = @Id";
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var param = new { Id };
                    this.LogQuery(SQLQuery, param);
                    IEnumerable<User> users = dbConnection.Query<User>(SQLQuery, param)
                        .ToList();
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByProjectID)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetByProjectID)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        //public bool CheckExistUser(User user)
        //{
        //    string SQLQuery = "SELECT * FROM users WHERE name_text = @Name_text AND password_text = @Password_text";
        //    try
        //    {
        //        User existUser = null;
        //        using (var dbConnection = new NpgsqlConnection(connectionString))
        //        {
        //            var param = new { user.Name_text, user.Password_text };
        //            this.LogQuery(SQLQuery, param);
        //            existUser = dbConnection.Query<User>(SQLQuery, param).FirstOrDefault();
        //            if (existUser == null)
        //                return true;
        //            return false;
        //        }
        //    }
        //    catch (NpgsqlException exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CheckExistUser)} {nameof(NpgsqlException)} ",
        //            exception);
        //        return false;
        //    }
        //    catch (Exception exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CheckExistUser)} {nameof(Exception)} ",
        //            exception);
        //        return false;
        //    }


        //}

        public IEnumerable<User> GetAll()
        {
            string SQLQuery = "SELECT * FROM users";
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    this.LogQuery(SQLQuery);
                    IEnumerable<User> users = dbConnection.Query<User>(SQLQuery).ToList();
                    return users;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAll)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAll)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        //public bool Remove(int Id)
        //{
        //    string SQLQuery = "DELETE FROM Users WHERE Id = @Id";
        //    try
        //    {
        //        using (var dbConnection = new NpgsqlConnection(connectionString))
        //        {

        //            var param = new { Id };
        //            this.LogQuery(SQLQuery, param);
        //            dbConnection.Execute(SQLQuery, param);
        //        }
        //        throw new NotImplementedException();
        //    }
        //    catch (NpgsqlException exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Remove)} {nameof(NpgsqlException)} ",
        //            exception);
        //        return false;
        //    }
        //    catch (Exception exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Remove)} {nameof(Exception)} ",
        //            exception);
        //        return false;
        //    }


        //}

        //public void Update(User user)
        //{
        //    string SQLQuery = "UPDATE users SET name_text = @Name_text, password_text = @Password_text, photo = @Photo, email = @Email";
        //    try
        //    {
        //        using (var dbConnection = new NpgsqlConnection(connectionString))
        //        {
        //            this.LogQuery(SQLQuery, user.GetType(), user);
        //            dbConnection.Execute(SQLQuery, user);
        //        }
        //        throw new NotImplementedException();
        //    }
        //    catch (NpgsqlException exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Update)} {nameof(NpgsqlException)} ",
        //            exception);
        //    }
        //    catch (Exception exception)
        //    {
        //        this._loggerError.WriteLn(
        //            $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.Update)} {nameof(Exception)} ",
        //            exception);
        //    }


        //}

        public async Task<byte[]> GetPhotoByIdAsync(Guid id)
        {
            try
            {

                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("users")
                        .Select("photo")
                        .Where("id", id);
                    var compiledQuery = this._compiler.Compile(query);
                    this.LogQuery(compiledQuery);
                    var userAvatar = await dbConnection.ExecuteScalarAsync<byte[]>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return userAvatar;
                }
            }
            catch (NpgsqlException exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetPhotoByIdAsync)} {nameof(NpgsqlException)} ",
                    exception);
                return null;
            }
            catch (Exception exception)
            {
                this._loggerError.WriteLn(
                    $"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetPhotoByIdAsync)} {nameof(Exception)} ",
                    exception);
                return null;
            }
        }

        //
        /// <summary>
        /// Проверка уникальности email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name_text"></param>
        /// <returns></returns>
        public async Task<bool?> IsUniqueEmail(string email, string name_text = null)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("users")
                        .Where("email", email);
                    if (name_text != null)
                        query = query.WhereNot("name_text", name_text);
                    query = query.AsCount();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return count == 0;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueEmail)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueEmail)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Проверка уникальности имени пользователя (логина).
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<bool?> IsUniqueLogin(string login)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("users")
                        .Where("name_text", login)
                        .AsCount();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    return (count == 0);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueLogin)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.IsUniqueLogin)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Смена пароля.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> PasswordChange(UserPasswordChangeDTO user)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    user.PasswordCurrent = Utilities.Cryptography.CryptographyProvider.GetMD5Hash(user.PasswordCurrent);

                    var query = new Query("users")
                        .Where("name_text", user.Name_text)
                        .Where("password_text", user.PasswordCurrent)
                        .AsCount();
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    if (count == 0)
                        return false;


                    user.PasswordNew = Utilities.Cryptography.CryptographyProvider.GetMD5Hash(user.PasswordNew);
                    var queryChange = new Query("users")
                        .Where("name_text", user.Name_text)
                        .AsUpdate(new { password_text = user.PasswordNew });
                    var compiledQueryChange = _compiler.Compile(queryChange);
                    LogQuery(compiledQueryChange);
                    await dbConnection.ExecuteAsync(
                        sql: compiledQueryChange.Sql,
                        param: compiledQueryChange.NamedBindings);
                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.PasswordChange)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.PasswordChange)} {nameof(Exception)} ", exception);
                return false;
            }

        }

        /// <summary>
        /// Восстановление пароля.
        /// </summary>
        /// <param name="name">имя пользователя (логин) или email</param>
        /// <returns></returns>
        public async Task<bool> RecoverPassword(string name)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("users")
                        .Where("name_text", name)
                        .OrWhere("email", name)
                        .Select("name_text", "email");
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var user = await dbConnection.QueryFirstOrDefaultAsync<User>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    if (user == null)
                        return false;


                    //Отправка инструкции по email для восстановления пароля.


                    return true;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.RecoverPassword)} {nameof(NpgsqlException)} ", exception);
                return false;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.RecoverPassword)} {nameof(Exception)} ", exception);
                return false;
            }
        }

        /// <summary>
        /// Регистрация. Создание пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Guid?> CreateUser(User user)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {


                    var newUser = new
                    {
                        name_text = user.Name_text,
                        email = user.Email,
                        password_text = Utilities.Cryptography.CryptographyProvider.GetMD5Hash(user.Password_text),
                        data_create = DateTime.Now
                    };
                    /* var query = new Query("users").AsInsert(newUser,
                         true); //true - вернуть сгенерированный id нового объекта
                     var compiledQuery = _compiler.Compile(query);
                     LogQuery(compiledQuery);

                     var idOfNewUser = await dbConnection
                         .ExecuteScalarAsync<Guid>(
                             sql: compiledQuery.Sql,
                             param: compiledQuery.NamedBindings);
                      */

                    var sql = "INSERT INTO users (name_text, email,password_text, data_create) VALUES('" +
                              newUser.name_text + "', " + newUser.email + ", " + newUser.password_text + ", " +
                              newUser.data_create + ") RETURNING  users.id";
                    var idOfNewUser = await dbConnection
                        .ExecuteScalarAsync<Guid>(sql);


                    ///создание записи в participants
                    var id = roles.GetRoleId("observer");
                    Participant newParticipant = new Participant();
                    newParticipant.ID_Localization_Project = null;
                    newParticipant.Active = true;
                    newParticipant.ID_Role = (Guid)id;
                    newParticipant.ID_User = idOfNewUser;

                    participant.AddAsync(newParticipant);


                    ////

                    return idOfNewUser;
                }

            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CreateUser)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.CreateUser)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        public async Task<String> GetRoleAsync(string userName, Guid? projectId)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (projectId == null)
                    {
                        string SQLQuery = @"SELECT roles.name_text 
                                      FROM users 
                                      INNER JOIN participants as p ON p.id_user = users.id 
                                      INNER JOIN roles ON roles.id = p.id_role 
                                      WHERE users.name_text = @userName";

                        var param = new { userName };
                        this.LogQuery(SQLQuery, param);
                        var userRole = await dbConnection.QuerySingleOrDefaultAsync<string>(SQLQuery, param);
                        return userRole;
                    }
                    else
                    {
                        string SQLQuery = @"SELECT roles.name_text 
                                      FROM users 
                                      INNER JOIN participants as p ON p.id_user = users.id 
                                      INNER JOIN roles ON roles.id = p.id_role 
                                      WHERE users.name_text = @userName AND p.id_localization_project = @projectId ";

                        var param = new { userName, projectId };
                        this.LogQuery(SQLQuery, param);
                        var userRole = await dbConnection.QuerySingleOrDefaultAsync<string>(SQLQuery, param);
                        return userRole;
                    }
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.LoginAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.LoginAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="user">логин и пароль.</param>
        /// <returns></returns>
        public async Task<User> LoginAsync(User user)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    user.Password_text = Utilities.Cryptography.CryptographyProvider.GetMD5Hash(user.Password_text);
                    string SQLQuery = @"SELECT u.id as id, 
                                      u.name_text as Name_text, 
                                      u.password_text as Password_text, 
                                      u.photo as Photo, 
                                      u.email as Email, 
                                      u.joined as Joined, 
                                      'Наблюдатель' as Role 
                                      FROM users  as u                                      
                                      WHERE (u.name_text = @Name_text OR email = @Email) AND password_text = @Password_text  limit 1";

                    var param = new { user.Name_text, user.Email, user.Password_text };
                    this.LogQuery(SQLQuery, param);
                    var existedUser = await dbConnection.QueryFirstOrDefaultAsync<User>(SQLQuery, param);
                    return existedUser;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.LoginAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.LoginAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Получение профиля пользователя.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<UserProfileForEditingDTO> GetProfileAsync(string name)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = new Query("users")
                        .Where("users.name_text", name)
                        .LeftJoin("users_locales", "users_locales.id_user", "users.id")
                        .Select(
                        "users.*",
                        "users_locales.id_locale As LocaleId",
                        "users_locales.is_native As LocaleIsNative"
                        );
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    var temp = await dbConnection.QueryAsync<UserProfile>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings);

                    //Создание пользователя с вложенными списками идентификаторов связанных данных.
                    var resultDTO = new UserProfileForEditingDTO
                    {
                        id = (Guid)temp.FirstOrDefault()?.id,
                        name_text = temp.FirstOrDefault().name_text,
                        email = temp.FirstOrDefault().email,
                        photo = temp.FirstOrDefault().photo,
                        full_name = temp.FirstOrDefault().full_name,
                        about_me = temp.FirstOrDefault().about_me,
                        gender = temp.FirstOrDefault().gender,
                        id_time_zones = temp.FirstOrDefault().id_time_zones,

                        locales_id_is_native = temp.Count(t => t.LocaleId != null) > 0
                        ? temp.Select(t => Tuple.Create<Guid, bool>(t.LocaleId.Value, t.LocaleIsNative)).Distinct()
                        : null
                    };

                    return resultDTO;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetProfileAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetProfileAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Получение профиля пользователя.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Guid? GetID(string name)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var query = "SELECT users.id FROM users WHERE users.name_text = '" + name + "'";


                    this.LogQuery(query);
                    var idOfInsertedRow = dbConnection.ExecuteScalar<Guid>(query);
                    return idOfInsertedRow;

                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetID)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetID)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Сохранение изменений в профиле пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UserProfileForEditingDTO user)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var edited = new
                    {
                        //user.photo,
                        user.email,
                        //user.joined,
                        user.full_name,
                        user.id_time_zones,
                        user.about_me,
                        user.gender,
                        date_change = DateTime.Now
                    };
                    var query = new Query("users")
                        .Where("users.name_text", user.name_text)
                        .AsUpdate(edited);
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);
                    await dbConnection.ExecuteAsync(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);

                    var queryPhoto = "UPDATE users SET photo = @photo WHERE users.name_text = @name_text";
                    LogQuery(queryPhoto, new { photo = user.photo, name_text = user.name_text });
                    await dbConnection.ExecuteAsync(queryPhoto, new { photo = user.photo, name_text = user.name_text });


                    var queryId = new Query("users")
                        .Where("users.name_text", user.name_text)
                        .Select("users.id");
                    var compiledQueryId = _compiler.Compile(queryId);
                    LogQuery(compiledQueryId);
                    var id = await dbConnection.QueryFirstOrDefaultAsync<Guid>(
                        sql: compiledQueryId.Sql,
                        param: compiledQueryId.NamedBindings);

                    //Пересоздание связей пользователя с языками перевода (Users с Locales)
                    await UpdateUsersLocalesAsync(id, user.locales_id_is_native);
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Пересоздание связей пользователя с языками перевода (Users с Locales).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="localesIds">Выбранные языки перевода.</param>
        /// <param name="isDeleteOldRecords">Удалить старые записи.</param>
        /// <returns></returns>
        public async Task UpdateUsersLocalesAsync(Guid userId, IEnumerable<Tuple<Guid, bool>> localesIdIsNative, bool isDeleteOldRecords = true)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    if (isDeleteOldRecords)
                    {
                        var queryDelete = new Query("users_locales")
                            .Where("id_user", userId)
                            .AsDelete();
                        var compiledQueryDelete = _compiler.Compile(queryDelete);
                        LogQuery(compiledQueryDelete);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryDelete.Sql,
                            param: compiledQueryDelete.NamedBindings);
                    }


                    var usersLocalesIsNative = localesIdIsNative.Select(t => new
                    {
                        id_user = userId,
                        id_locale = t.Item1,
                        is_native = t.Item2
                    }).ToList();

                    foreach (var element in usersLocalesIsNative)
                    {
                        var queryInsert = new Query("users_locales").AsInsert(element);
                        var compiledQueryInsert = _compiler.Compile(queryInsert);
                        LogQuery(compiledQueryInsert);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQueryInsert.Sql,
                            param: compiledQueryInsert.NamedBindings);
                    }

                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateUsersLocalesAsync)} {nameof(NpgsqlException)} ", exception);
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.UpdateUsersLocalesAsync)} {nameof(Exception)} ", exception);
            }
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="name">логин авторизованного пользователя.</param>
        /// <returns></returns>
        public async Task<bool?> RemoveAsync(string name)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    //Удаление аккаунта возможно, если пользователь не является владельцем ни одного проекта
                    var isOwner = await _participantsRepository.IsOwnerInAnyProject(name);

                    if (isOwner == false)
                    {
                        var query = new Query("users")
                            .Where("users.name_text", name)
                            .AsDelete();
                        var compiledQuery = _compiler.Compile(query);
                        LogQuery(compiledQuery);
                        await dbConnection.ExecuteAsync(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);
                        return true;
                    }

                    return false;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.RemoveAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.RemoveAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        //
        public static Dictionary<string, string> SortColumnNamesMapping = new Dictionary<string, string>()
        {
            { "id", "users.id" },
            { "wordsQuantity", "" },
            { "cost", "" }
        };

        /// <summary>
        /// Возвращает строки (со связанными объектами).
        /// </summary>
        /// <param name="currentLanguagesId">Идентификатор языка оригинала.</param>
        /// <param name="translateLanguagesId">Идентификатор языка перевода.</param>
        /// <param name="nativeLanguage">Флаг родной язык, указанный язык перевода.</param>
        /// <param name="servicesId">Идентификатор услуги.</param> 
        /// <param name="topicsId">Идентификаторы тематик.</param>
        /// <param name="minPrice">Ставка за слово минимальная.</param>
        /// <param name="maxPrice">Ставка за слово максимальная.</param> 
        /// <param name="offset">Количество пропущенных строк.</param>
        /// <param name="limit">Количество возвращаемых строк.</param>
        /// <param name="sortBy">Имя сортируемого столбца.</param>
        /// <param name="sortAscending">Порядок сортировки.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Translator>> GetAllTranslatorsAsync(
            Guid? currentLanguagesId,
            Guid? translateLanguagesId,
            bool? nativeLanguage,
            Guid? servicesId,
            Guid[] topicsId,
            int? minPrice,
            int? maxPrice,
            int offset,
            int limit,
            string[] sortBy = null,
            bool sortAscending = true)
        {
            if (sortBy == null || !sortBy.Any())
            {
                sortBy = new[] { "id" };
            }

            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = GetAllTranslatorsQuery(
                        currentLanguagesId,
                        translateLanguagesId,
                        nativeLanguage,
                        servicesId,
                        topicsId,
                        minPrice,
                        maxPrice);

                    query = ApplyPagination(
                        query: query,
                        offset: offset,
                        limit: limit);

                    query = ApplySorting(
                        query: query,
                        columnNamesMappings: UserRepository.SortColumnNamesMapping,
                        sortBy: sortBy,
                        sortAscending: sortAscending);

                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    var translators = await dbConnection.QueryAsync<Translator>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );

                    return translators;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAllTranslatorsAsync)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAllTranslatorsAsync)} {nameof(Exception)} ", exception);
                return null;
            }
        }

        /// <summary>
        /// Возвращает количество строк.
        /// </summary>
        /// <param name="currentLanguagesId">Идентификатор языка оригинала.</param>
        /// <param name="translateLanguagesId">Идентификатор языка перевода.</param>
        /// <param name="nativeLanguage">Флаг родной язык, указанный язык перевода.</param>
        /// <param name="servicesId">Идентификатор услуги.</param> 
        /// <param name="topicsId">Идентификаторы тематик.</param>
        /// <param name="minPrice">Ставка за слово минимальная.</param>
        /// <param name="maxPrice">Ставка за слово максимальная.</param> 
        /// <returns></returns>
        public async Task<int> GetAllTranslatorsCountAsync(
            Guid? currentLanguagesId = null,
            Guid? translateLanguagesId = null,
            bool? nativeLanguage = null,
            Guid? servicesId = null,
            Guid[] topicsId = null,
            int? minPrice = null,
            int? maxPrice = null)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {
                    var query = GetAllTranslatorsQuery(
                        currentLanguagesId,
                        translateLanguagesId,
                        nativeLanguage,
                        servicesId,
                        topicsId,
                        minPrice,
                        maxPrice);
                    query = query.Distinct().AsCount("users.id");


                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    var count = await dbConnection.ExecuteScalarAsync<int>(
                        sql: compiledQuery.Sql,
                        param: compiledQuery.NamedBindings
                    );

                    return count;
                }
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAllTranslatorsCountAsync)} {nameof(NpgsqlException)} ", exception);
                return 0;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAllTranslatorsCountAsync)} {nameof(Exception)} ", exception);
                return 0;
            }

        }

        /// <summary>
        /// Возвращает запрос строк (со связанными объектами).
        /// </summary>
        /// <param name="currentLanguagesId">Идентификатор языка оригинала.</param>
        /// <param name="translateLanguagesId">Идентификатор языка перевода.</param>
        /// <param name="nativeLanguage">Флаг родной язык, указанный язык перевода.</param>
        /// <param name="servicesId">Идентификатор услуги.</param> 
        /// <param name="topicsId">Идентификаторы тематик.</param>
        /// <param name="minPrice">Ставка за слово минимальная.</param>
        /// <param name="maxPrice">Ставка за слово максимальная.</param> 
        /// <returns></returns>
        private Query GetAllTranslatorsQuery(
            Guid? currentLanguagesId = null,
            Guid? translateLanguagesId = null,
            bool? nativeLanguage = null,
            Guid? servicesId = null,
            Guid[] topicsId = null,
            int? minPrice = null,
            int? maxPrice = null)
        {
            try
            {
                var queryUsersTranslationTopics = new Query("users")
                    .LeftJoin("users_translation_topics", "users_translation_topics.id_user", "users.id")
                    .LeftJoin("translation_topics", "translation_topics.id", "users_translation_topics.id_translation_topics")
                    .WhereTrue("users.public_profile")
                    .Select("users.id as id_user")
                    .GroupBy("users.id")
                    .SelectRaw("string_agg(translation_topics.name_text, ', ' order by translation_topics.name_text) as translation_topics_name");

                var queryUsersTranslationServices = new Query("users")
                    .LeftJoin("translation_services", "translation_services.id_user", "users.id")
                    .LeftJoin("type_of_service", "type_of_service.id", "translation_services.id_type_of_service")
                    .LeftJoin("currency", "currency.id", "translation_services.id_currency")
                    .WhereTrue("users.public_profile")
                    .SelectRaw("DISTINCT on (users.id) users.id AS id_user, type_of_service.name_text AS service_name, translation_services.price, currency.name_text AS currency_name");
                //.Select("users.id as id_user", "type_of_service.name_text as service_name", "translation_services.price", "currency.name_text as currency_name")
                //.Take(1);
                //.GroupBy("translation_services.id")
                //.Distinct();

                //Добавить запрос на вычисление кол-ва переведенных слов.

                var query = new Query("users")
                    .With("users_topics", queryUsersTranslationTopics)
                    .With("users_services", queryUsersTranslationServices)
                    .Join("users_topics", "users_topics.id_user", "users.id")
                    .Join("users_services", "users_services.id_user", "users.id")
                    .LeftJoin("participants", "participants.id_user", "users.id")
                    .WhereTrue("users.public_profile")
                    .Select(
                        "users.id as user_Id",
                        "users.name_text as user_Name",
                        "users.photo as user_pic",
                        "users_topics.translation_topics_name",
                        "users_services.service_name as service",
                        "users_services.price as cost",
                        "users_services.currency_name as currency"//,
                                                                  //"participants.deadlines",//заменить на вычисление среднего значения
                                                                  //"participants.quality_of_work"//заменить на вычисление среднего значения
                                                                  //вставить кол-во переведенных слов.
                    )//.Distinct();
                    .GroupBy(
                        "users.id",
                        "users.name_text",
                        "users.photo",
                        "users_topics.translation_topics_name",
                        "users_services.service_name",
                        "users_services.price",
                        "users_services.currency_name")
                    .SelectRaw("avg(participants.deadlines) as termRating")
                    .SelectRaw("avg(participants.quality_of_work) as translationRating");

                if (currentLanguagesId != null)
                {
                    //query = query.Join("translation_memories_strings", "translation_memories_strings.id_string", "translation_substrings.id")
                    //             .Where("translation_memories_strings.id_translation_memory", translationMemoryId);
                }
                else
                {
                    //query = query.Join("translation_memories_strings", "translation_memories_strings.id_string", "translation_substrings.id")
                    //             .Join("localization_projects_translation_memories", "localization_projects_translation_memories.id_translation_memory", "translation_memories_strings.id_translation_memory")
                    //             .Where("localization_projects_translation_memories.id_localization_project", projectId);
                }

                var compiledQuery = _compiler.Compile(query);
                LogQuery(compiledQuery);

                return query;
            }
            catch (NpgsqlException exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAllTranslatorsQuery)} {nameof(NpgsqlException)} ", exception);
                return null;
            }
            catch (Exception exception)
            {
                _loggerError.WriteLn($"Ошибка в {nameof(UserRepository)}.{nameof(UserRepository.GetAllTranslatorsQuery)} {nameof(Exception)} ", exception);
                return null;
            }
        }

    }
}
