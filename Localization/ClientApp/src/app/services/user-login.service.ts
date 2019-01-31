import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../user-account/user';

@Injectable()
export class UserLoginService {
  private url = "../api/user-account.user";

  constructor(private http: HttpClient) {

  }

  getUsers() {
    return this.http.get(this.url);
  }

  getUser(Id: number) {
    return this.http.get(this.url)
  }


  createUsers(user: User) {
    return this.http.post(this.url, user);
  }

  updateUsers(user: User) {
    return this.http.post(this.url + '/' + user.id, user);
  }

  deleteUser(id: number) {
    return this.http.delete(this.url + '/' + id);
  }
}

