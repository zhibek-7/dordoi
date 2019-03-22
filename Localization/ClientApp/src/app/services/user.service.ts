import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from "@angular/common/http";
import { User } from "../models/database-entities/user.type";
import { Observable } from "rxjs";
import { UserProfile } from "../models/DTO/userProfile.type";
import { userPasswordChange } from "../account/models/userPasswordChange.model";
import { Translator } from "../models/Translators/translator.type";
import { Guid } from "guid-typescript";

@Injectable()
export class UserService {
  private url: string = "api/User/";

  constructor(private httpClient: HttpClient) {}

  get currentUserName(): string {
    return sessionStorage.getItem("userName");
  }

  getUserList(): Observable<User[]> {
    return this.httpClient.post<User[]>(this.url + "List", null);
  }

  getProjectParticipantList(projectId: Guid): Observable<User[]> {
    return this.httpClient.post<User[]>(
      this.url + "List/projectId:" + projectId,
      null
    );
  }

  getPhotoById(userId: Guid): Observable<Blob> {
    return this.httpClient.post(
      this.url + userId.toString() + "/getPhoto/",
      null,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        ),
        responseType: "blob"
      }
    );
  }

  //
  /**
   * Проверка уникальности email.
   * @param email введенный email.
   */
  isUniqueEmail(email: string): Observable<boolean> {
    return this.httpClient.post<boolean>(
      this.url + "isUniqueEmail:" + email,
      email
    );
  }

  /**
   * Проверка уникальности имени пользователя (логина).
   * @param login введенное имя пользователя (логин).
   */
  isUniqueLogin(login: string): Observable<boolean> {
    return this.httpClient.post<boolean>(
      this.url + "isUniqueLogin:" + login,
      login
    );
  }

  /**
   * Регистрация. Создание пользователя.
   * @param user
   */
  createUser(user: User): Observable<Object> {
    return this.httpClient.post<number>(this.url + "registration", user);
  }

  /**
   * Авторизация.
   * @param user логин и пароль.
   */
  login(user: User): any {
    return this.httpClient.post(this.url + "login", user);
  }

  /**
   * Смена пароля.
   * @param user текущий и новый пароли.
   */
  passwordChange(user: userPasswordChange): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url + "passwordChange", user);
  }

  /**
   * Восстановление пароля.
   * @param name имя пользователя (логин) или email
   */
  recoverPassword(name: string): Observable<boolean> {
    return this.httpClient.post<boolean>(
      this.url + "recoverPassword:" + name,
      name
    );
  }

  /** Получение профиля пользователя. */
  getProfile(): Observable<UserProfile> {
    return this.httpClient.post<UserProfile>(this.url + "profile", null);
  }

  /**
   * Сохранение изменений в профиле пользователя.
   * @param user
   */
  toSaveEditedProfile(user: UserProfile): Observable<Object> {
    return this.httpClient.post(this.url + "toSaveEdited", user);
  }

  /** Удаление пользователя. */
  delete(): Observable<boolean> {
    let id = sessionStorage.getItem("currentUserID");
    return this.httpClient.delete<boolean>(this.url + "delete/" + id);
  }
  
  /**
   * Возвращает список пользователей (переводчиков), со строками перечислений имен связанных объектов.
   * @param currentLanguagesId Идентификатор языка оригинала.
   * @param translateLanguagesId Идентификатор языка перевода.
   * @param nativeLanguage Флаг родной язык, указанный язык перевода.
   * @param servicesId Идентификатор услуги.
   * @param topicsId Идентификаторы тематик.
   * @param minPrice Ставка за слово минимальная.
   * @param maxPrice Ставка за слово максимальная.
   * @param limit Количество возвращаемых строк.
   * @param offset Количество пропущенных строк.
   * @param sortBy Имя сортируемого столбца.
   * @param sortAscending Порядок сортировки.
   */
  getAllTranslators(
    currentLanguagesId?: Guid,
    translateLanguagesId?: Guid,
    nativeLanguage?: boolean,
    servicesId?: Guid,
    topicsId?: Guid[],
    minPrice?: number,
    maxPrice?: number,
    limit?: number,
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean): Observable<HttpResponse<Translator[]>> {
    let params = new HttpParams();

    if (currentLanguagesId) {
      params = params.set('currentLanguagesId', currentLanguagesId.toString());
    }
    if (translateLanguagesId) {
      params = params.set('translateLanguagesId', translateLanguagesId.toString());
    }
    if (nativeLanguage != null) {
      params = params.set('nativeLanguage', nativeLanguage.toString());
    }
    if (servicesId) {
      params = params.set('servicesId', servicesId.toString());
    }
    if (topicsId && topicsId.length > 0) {
      topicsId.forEach(topicsIdItem => params = params.append('topicsId', topicsIdItem.toString()));
    }
    if (minPrice) {
      params = params.set('minPrice', minPrice.toString());
    }
    if (maxPrice) {
      params = params.set('maxPrice', maxPrice.toString());
    }
    if (limit) {
      params = params.set('limit', limit.toString());
    }
    if (offset) {
      params = params.set('offset', offset.toString());
    }
    if (sortBy && sortBy.length > 0) {
      sortBy.forEach(sortByItem => params = params.append('sortBy', sortByItem));
      if (sortAscending !== undefined) {
        params = params.set('sortAscending', sortAscending.toString());
      }
    }
    return this.httpClient.post<Translator[]>(this.url + "ListTranslators", null,
      {
        params: params,
        observe: 'response'
      });
  }
}

