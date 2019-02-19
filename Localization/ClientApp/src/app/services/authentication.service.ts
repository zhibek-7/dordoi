import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of as observableOf  } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { UserProfile } from '../models/DTO/userProfile.type';

@Injectable()
export class AuthenticationService {

  private authTokenStale: string = 'stale_auth_token';
  private authTokenNew: string = 'new_auth_token';

  private url: string = 'api/User/';

  userAuthorized: boolean;
  currentUserName: string = "";

  constructor(private http: HttpClient) { }

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

  saveUserName(initUserName: string){
    this.currentUserName = initUserName;
  }

  refreshTokenRequest(): any {
    return this.http.post(this.url + "refreshToken", null, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }  

  refreshToken(): Observable<string> {
    /*
        The call that goes in here will use the existing refresh token to call
        a method on the oAuth server (usually called refreshToken) to get a new
        authorization token for the API calls.
    */
    return this.http.post(this.url + "refreshToken", null, {
          headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
        }).pipe(map(data => {
          let currentToken = data["token"];
          this.saveToken(data["token"]); 
          // console.log(currentToken);
          return currentToken;
        }));

    this.refreshTokenRequest().subscribe(
      response => {
        this.saveToken(response.token);        
      }
    ); 
    

    // // Just to keep HttpClient from getting tree shaken.
    // this.http.get('http://private-4002d-testerrorresponses.apiary-mock.com/getData');

    return observableOf(this.getToken()).pipe(delay(2000));
}

}