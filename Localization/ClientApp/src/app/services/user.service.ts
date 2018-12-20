import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/database-entities/user.type';
import { Observable, of } from 'rxjs';

@Injectable()
export class UserService {

  private url: string = "api/User/";

  constructor(private httpClient: HttpClient) { }

  getUserList(): Observable<User[]> {
    return this.httpClient.get<User[]>(this.url + "List");
  }

  getPhotoById(userId: number): Observable<Blob> {
    return this.httpClient.get(this.url + userId.toString() + '/getPhoto/', { responseType: 'blob' });
  }

}
