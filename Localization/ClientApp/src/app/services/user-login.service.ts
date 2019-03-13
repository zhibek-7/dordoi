import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "../user-account/user";

@Injectable()
export class UserLoginService {
  private url = "../api/user-account.user";

  constructor(private http: HttpClient) {}

  getUsers() {
    return this.http.post(this.url, null);
  }

  getUser(Id: number) {
    return this.http.post(this.url, Id);
  }

  createUsers(user: User) {
    return this.http.post(this.url, user);
  }

  updateUsers(user: User) {
    return this.http.post(this.url + "/" + user.id, user);
  }

  deleteUser(id: number) {
    return this.http.delete(this.url + "/" + id);
  }
}
