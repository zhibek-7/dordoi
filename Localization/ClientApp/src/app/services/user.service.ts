import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/database-entities/user.type';
import { Observable, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserProfile } from '../models/DTO/userProfile.type';
import { userPasswordChange } from '../account/models/userPasswordChange.model';

@Injectable()
export class UserService {

  private url: string = 'api/User/';

  constructor(private httpClient: HttpClient) { }

  get currentUserId(): number {
    return +sessionStorage.getItem('currentUserID');
  }

  get currentUserName(): string {
    return sessionStorage.getItem('currentUserName');
  }

  getUserList(): Observable<User[]> {
    return this.httpClient.post<User[]>(this.url + "List", null);
  }

  getProjectParticipantList(projectId: number): Observable < User[] > {
    return this.httpClient.post<User[]>(this.url + "List/projectId:" + projectId, null);
  }

  getPhotoById(userId: number): Observable<Blob> {
    return this.httpClient.post(this.url + userId.toString() + '/getPhoto/', null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken")),
    responseType: 'blob' });
  }

  //
  /**
   * Проверка уникальности email.
   * @param email введенный email.
   */
  isUniqueEmail(email: string): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url + "isUniqueEmail:" + email, email);
  }

  /**
   * Проверка уникальности имени пользователя (логина).
   * @param login введенное имя пользователя (логин).
   */
  isUniqueLogin(login: string): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url + "isUniqueLogin:" + login, login);
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
    return this.httpClient.post<boolean>(this.url + "recoverPassword:" + name, name);
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
    let id = sessionStorage.getItem('currentUserID');
    return this.httpClient.delete<boolean>(this.url + "delete");
  }  

}
