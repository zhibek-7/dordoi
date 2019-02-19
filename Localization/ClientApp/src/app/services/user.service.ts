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

  private userName: string = null;

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
    return this.httpClient.post(this.url + userId.toString() + '/getPhoto/', null, { responseType: 'blob' });
  }

  //
  /**
   * Проверка уникальности email.
   * @param email введенный email.
   */
  isUniqueEmail(email: string): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url + "isUniqueEmail:" + email, email, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
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
    return this.httpClient.post<boolean>(this.url + "passwordChange", user, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    }); 
  }

  /** Получение профиля пользователя. */
  getProfile(): Observable<UserProfile> {
    return this.httpClient.post<UserProfile>(this.url + "profile", null, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  /**
   * Сохранение изменений в профиле пользователя.
   * @param user
   */
  toSaveEditedProfile(user: UserProfile): Observable<Object> {
    return this.httpClient.post(this.url + "toSaveEdited", user, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  /** Удаление пользователя. */
  delete(): Observable<boolean> {
    let id = sessionStorage.getItem('currentUserID');
    return this.httpClient.delete<boolean>(this.url + "delete", {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  getUserName(): string {
    return this.userName;
  }

  setUserName(userName: string) {
    this.userName = userName;
  }

  getUserRole(): string {
     return sessionStorage.getItem('userRole');
  }

  setUserRole(userRole: string) {
    sessionStorage.setItem('userRole', userRole);
  }


}
