import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { User } from '../user-account/user';

@Injectable()
export class UserRegistrationService {
  private url = "../api/user-account.user";

  constructor(private http: HttpClient) {

  }

  getUsers() {
    return this.http.get(this.url,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  getUser(Id: number) {
    return this.http.get(this.url,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))
    })
  }


  createUsers(user: User) {
    return this.http.post(this.url, user);
  }

  updateUsers(user: User) {
    return this.http.post(this.url + '/' + user.id, user,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  deleteUser(id: number) {
    return this.http.delete(this.url + '/' + id,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))
    });
  }
}

