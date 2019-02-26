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
    public class UserRepository : BaseRepository, IRepository<User>
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

        public IEnumerable<User> GetByProjectID(int Id)
        {
            try
            {
                string SQLQuery = "SELECT u.* FROM users u " +
                        " join participants p on u.id = p.id_user " +
                        " join localization_projects lp on p.id_localization_project = lp.id " +
                        " where lp.id = @Id";
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

        public async Task<byte[]> GetPhotoByIdAsync(int id)
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

                    return count == 0;
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
        public async Task<int?> CreateUser(User user)
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
                    var query = new Query("users").AsInsert(newUser,
                        true); //true - вернуть сгенерированный id нового объекта
                    var compiledQuery = _compiler.Compile(query);
                    LogQuery(compiledQuery);

                    var idOfNewUser = await dbConnection
                        .ExecuteScalarAsync<int>(
                            sql: compiledQuery.Sql,
                            param: compiledQuery.NamedBindings);


                    ///создание записи в participants
                    var id = roles.GetRoleId("observer");
                    Participant newParticipant = new Participant();
                    newParticipant.ID_Localization_Project = null;
                    newParticipant.Active = true;
                    newParticipant.ID_Role = (int)id;
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
                    string SQLQuery = "SELECT users.id as id, " +
                                      "users.name_text as Name_text, " +
                                      "users.password_text as Password_text, " +
                                      "users.photo as Photo, " +
                                      "users.email as Email, " +
                                      "users.joined as Joined, " +
                                      "roles.name_text as Role " +
                                      "FROM users " +
                                      "INNER JOIN participants ON participants.id_user = users.id " +
                                      "INNER JOIN roles ON roles.id = participants.id_role " +
                                      "WHERE (users.name_text = @Name_text OR email = @Email) AND password_text = @Password_text";

                    var param = new { user.Name_text, user.Email, user.Password_text };
                    this.LogQuery(SQLQuery, param);
                    var existedUser = await dbConnection.QuerySingleOrDefaultAsync<User>(SQLQuery, param);
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
                        //id = temp.FirstOrDefault().id,
                        name_text = temp.FirstOrDefault().name_text,
                        email = temp.FirstOrDefault().email,
                        photo = temp.FirstOrDefault().photo,
                        full_name = temp.FirstOrDefault().full_name,
                        about_me = temp.FirstOrDefault().about_me,
                        gender = temp.FirstOrDefault().gender,
                        id_time_zones = temp.FirstOrDefault().id_time_zones,
                        //id = temp.FirstOrDefault().id,

                        locales_id_is_native = temp.Count(t => t.LocaleId != null) > 0
                        ? temp.Select(t => Tuple.Create<int, bool>(t.LocaleId.Value, t.LocaleIsNative)).Distinct()
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
        public int? GetID(string name)
        {
            try
            {
                using (var dbConnection = new NpgsqlConnection(connectionString))
                {

                    var query = "SELECT users.id FROM users WHERE users.name_text = '" + name + "'";


                    this.LogQuery(query);
                    var idOfInsertedRow = dbConnection.ExecuteScalar<int>(query);
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
                    var id = await dbConnection.QueryFirstOrDefaultAsync<int>(
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
        public async Task UpdateUsersLocalesAsync(int userId, IEnumerable<Tuple<int, bool>> localesIdIsNative, bool isDeleteOldRecords = true)
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
    }
}
