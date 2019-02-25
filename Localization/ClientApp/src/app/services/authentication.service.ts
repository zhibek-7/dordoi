import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of as observableOf  } from 'rxjs';
import { delay, map } from 'rxjs/operators';

import { UserService } from './user.service';

@Injectable()
export class AuthenticationService {

  private authTokenStale: string = 'stale_auth_token';
  private authTokenNew: string = 'new_auth_token';

  private url: string = 'api/User/';

  userAuthorized: boolean;

  currentUserName: string = "";

  constructor(private http: HttpClient,
              private userService: UserService) { }

  async checkUserAuthorisation() {    
    this.userAuthorized = await this.http.post<boolean>(this.url + "checkUserAuthorisation", null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    }).toPromise();       

    return await this.userAuthorized;           
  }

  getToken(): string {
    return sessionStorage.getItem("userToken");
  }

  saveToken(token: string){
    sessionStorage.setItem('userToken', token);  
  }
  
  deleteToken(){
    sessionStorage.clear();
  }  

  refreshTokenRequest(): any {
    return this.http.post(this.url + "refreshToken", null, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }  

  getUserName(): string {
    return this.currentUserName;
  }

  setUserName(userName: string) {
    this.currentUserName = userName;
  }

  getUserRole(): string {
     return sessionStorage.getItem('userRole');
  }

  setUserRole(userRole: string) {
    sessionStorage.setItem('userRole', userRole);
  }

  refreshToken(): Observable<string> {  

    let userName = this.getUserName();
    let userRole = this.getUserRole();

    const params = new HttpParams().set('userName', userName.toString()).set('userRole', userRole.toString());

    return this.http.post(this.url + "refreshToken", params , {
          headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
        }).pipe(map(data => {
          let currentToken = data["token"];
          this.saveToken(data["token"]); 
          return currentToken;
        }));
  }

}