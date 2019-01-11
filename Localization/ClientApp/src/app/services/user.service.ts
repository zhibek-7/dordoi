import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/database-entities/user.type';
import { Observable, from } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class UserService {

  private url: string = "api/User/";

  constructor(private httpClient: HttpClient) { }

  getUserList(): Observable<User[]> {
    return this.httpClient.post<User[]>(this.url + "List", null);
  }
  
  getProjectParticipantList(projectId: number): Observable < User[] > {
    return this.httpClient.post<User[]>(this.url + "List/projectId:" + projectId, null);
  }

  getPhotoById(userId: number): Observable<Blob> {
    return this.httpClient.post(this.url + userId.toString() + '/getPhoto/', null, { responseType: 'blob' });
  }
}
