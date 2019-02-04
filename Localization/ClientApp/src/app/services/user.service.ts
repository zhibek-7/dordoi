import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/database-entities/user.type';
import { Observable, from } from 'rxjs';
import { map } from 'rxjs/operators';

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
    return this.httpClient.post(this.url + userId.toString() + '/getPhoto/', null, { responseType: 'blob' });
  }

  //
  isUniqueEmail(email: string): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url + "isUniqueEmail:" + email, email);
  }
  isUniqueLogin(login: string): Observable<boolean> {
    return this.httpClient.post<boolean>(this.url + "isUniqueLogin:" + login, login);
  }
  createUser(user: User): Observable<Object> {
    return this.httpClient.post<number>(this.url + "registration", user);    
  }
  login(user: User): Observable<User> {
    return this.httpClient.post<User>(this.url + "login", user);
  }

//
//sessionStorage.setItem('currentUserName', 'Иван Иванов');
//sessionStorage.setItem('currentUserID', '300');
//this.currentUserName = sessionStorage.getItem('currentUserName');
//
}
