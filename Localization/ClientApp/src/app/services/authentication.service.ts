import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthenticationService {

  private url: string = 'api/User/';

  userAuthorized: boolean;

  constructor(private http: HttpClient) { }

  async checkUserAuthorisation() {    
    this.userAuthorized = await this.http.post<boolean>(this.url + "checkUserAuthorisation", null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    }).toPromise();       

    return await this.userAuthorized;           
  }

  // getStatus(): boolean {    
  //   return this.userAuthorized;
  // }

  saveToken(token: string){
    sessionStorage.setItem('userToken', token);  
  }
  
  deleteToken(){
    sessionStorage.clear();
  }

}