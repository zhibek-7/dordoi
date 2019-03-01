import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of as observableOf  } from 'rxjs';
import { delay, map } from 'rxjs/operators';

import { UserService } from './user.service';

@Injectable()
export class AuthenticationService {

  private url: string = 'api/User/';

  private userAuthorized: boolean;

  constructor(private http: HttpClient) { }

  //Устаревший метод, но пока не удаляю
  async checkUserAuthorisation() {    
    this.userAuthorized = await this.http.post<boolean>(this.url + "checkUserAuthorisation", null).toPromise();       

    return await this.userAuthorized;           
  }

  checkUserAuthorisationAsync() {
    return this.http.post<boolean>(this.url + "checkUserAuthorisation", null);
  }

  getToken(): string {
    return sessionStorage.getItem("userToken");
  }

  saveToken(token: string){
    sessionStorage.setItem('userToken', token);  
  }
  
  logOut(){
    this.userAuthorized = false;
    sessionStorage.clear();
  }  

  authorizeUser(){
    this.userAuthorized = true;
  }

  isLoggedIn(): boolean {
    if(this.userAuthorized){
      return true;
    } else {
      return false;
    }
  }

  refreshTokenRequest(): any {
    return this.http.post(this.url + "refreshToken", null);
  }  

  getUserName(): string {    
    return sessionStorage.getItem('userName');
  }

  setUserName(userName: string) {    
    sessionStorage.setItem('userName', userName);
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

    const params = new HttpParams().set('userName', userName).set('userRole', userRole);

    return this.http.post(this.url + "refreshToken", params).pipe(map(data => {
          let currentToken = data["token"];
          this.saveToken(data["token"]); 
          return currentToken;
        }));
  }

  setUserRoleAccordingToProject() {

    let projectId = sessionStorage.getItem('ProjectID');

    const params = new HttpParams().set('ProjectID', projectId);

    return this.http.post<string>(this.url + "setUserRoleAccordingToProject", params).pipe(map(data => {
          let currentRole = data["role"];
          this.setUserRole(currentRole);
          return currentRole;
        }));;
  }

  provideAccessOnlyFor(acceptableRoles: string[]): boolean {

    let accessProvided = false;
    let currentRole = this.getUserRole();


    acceptableRoles.some(function(item){
      if(item == currentRole){
        accessProvided = true;
        return accessProvided;
      }
    });

    return accessProvided;
  }

}